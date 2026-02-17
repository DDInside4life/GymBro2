using GymBro.App.Commands;
using GymBro.Business.Managers;
using System;
using System.Windows.Input;

namespace GymBro.App.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly UserManager _userManager;
        private string _login;
        private string _password;
        private string _confirmPassword;
        private string _email;
        private string _fullName;
        private string _errorMessage;

        public RegisterViewModel(UserManager userManager)
        {
            _userManager = userManager;
            RegisterCommand = new RelayCommand(ExecuteRegister, CanExecuteRegister);
            CancelCommand = new RelayCommand(ExecuteCancel);
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        public string Login { get => _login; set => SetProperty(ref _login, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        public string ConfirmPassword { get => _confirmPassword; set => SetProperty(ref _confirmPassword, value); }
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }
        public string ErrorMessage { get => _errorMessage; set => SetProperty(ref _errorMessage, value); }

        public ICommand RegisterCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand LoginCommand { get; }

        private bool CanExecuteRegister(object param)
        {
            return !string.IsNullOrWhiteSpace(Login) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !string.IsNullOrWhiteSpace(ConfirmPassword) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   Password == ConfirmPassword;
        }

        private void ExecuteRegister(object param)
        {
            try
            {
                var user = _userManager.Register(Login, Password, Email, FullName);
                RegistrationSuccessful?.Invoke(this, user);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private void ExecuteCancel(object param)
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }

        private void ExecuteLogin(object param)
        {
            LoginRequested?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<Domain.Entities.User> RegistrationSuccessful;
        public event EventHandler CancelRequested;
        public event EventHandler LoginRequested;
    }
}