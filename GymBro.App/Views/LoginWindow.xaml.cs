using GymBro.App.ViewModels;
using GymBro.Business.Infrastructure;
using GymBro.Business.Managers;
using System.Windows;
using GymBro.App.Views;

namespace GymBro.App.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            var factory = new ManagersFactory();
            var userManager = factory.GetUserManager();
            var vm = new LoginViewModel(userManager);
            vm.CloseRequested += (s, result) => { DialogResult = result; Close(); };
            vm.RegisterRequested += (s, e) =>
            {
                var registerWindow = new RegisterWindow();
                registerWindow.Owner = this;
                registerWindow.ShowDialog();
                // после закрытия регистрации можно остаться на логине
            };
            DataContext = vm;
        }
    }
}