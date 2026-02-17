using GymBro.App.Commands;
using GymBro.App.Infrastructure;
using System;
using System.Windows.Input;



namespace GymBro.App.ViewModels
{

    public class MainWindowViewModel : ViewModelBase
    {

        public event EventHandler<string> NavigationRequested;

        private ICommand _navigateCommand;
        public ICommand NavigateCommand => _navigateCommand ??= new RelayCommand(ExecuteNavigate);

        public string CurrentUserName => SessionManager.CurrentUser?.FullName ?? "Гость";
        public bool IsAdmin => SessionManager.IsInRole("Admin");
        
        private void ExecuteNavigate(object parameter)
        {
            string pageKey = parameter?.ToString();
            if (!string.IsNullOrEmpty(pageKey))
            {
                NavigationRequested?.Invoke(this, pageKey);
            }
        }
    }
}