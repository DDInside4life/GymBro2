using GymBro.App.Commands;
using GymBro.App.Infrastructure;
using GymBro.Business.Managers;
using System;
using System.Windows.Input;

namespace GymBro.App.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly UserManager _userManager;
        private string _login;
        private string _password;
        private string _errorMessage;



        public LoginViewModel(UserManager userManager)
        {
            _userManager = userManager;
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
            CancelCommand = new RelayCommand(ExecuteCancel);
            RegisterCommand = new RelayCommand(ExecuteRegister);
        }

        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand RegisterCommand { get; }

        private bool CanExecuteLogin(object param) =>
            !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);

        private void ExecuteLogin(object param)
        {
            try
            {
                var user = _userManager.ValidateUser(Login, Password);
                if (user != null)
                {
                    // Устанавливаем язык пользователя
                    if (!string.IsNullOrEmpty(user.Language))
                    {
                        System.Threading.Thread.CurrentThread.CurrentUICulture =
                            new System.Globalization.CultureInfo(user.Language);
                    }
                    SessionManager.Login(user);
                    CloseRequested?.Invoke(this, true);
                }
                else
                {
                    ErrorMessage = "Неверный логин или пароль";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Ошибка: " + ex.Message;
            }
        }

        private void ExecuteCancel(object param) =>
            CloseRequested?.Invoke(this, false);

        private void ExecuteRegister(object param) =>
            RegisterRequested?.Invoke(this, EventArgs.Empty);

        public event EventHandler<bool> CloseRequested;
        public event EventHandler RegisterRequested;
    }
}