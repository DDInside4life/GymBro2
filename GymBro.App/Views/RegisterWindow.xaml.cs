using GymBro.App.ViewModels;
using GymBro.Business.Infrastructure;
using GymBro.Business.Managers;
using System.Windows;

namespace GymBro.App.Views
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();

            var factory = new ManagersFactory();
            var userManager = factory.GetUserManager();
            var vm = new RegisterViewModel(userManager);
            vm.RegistrationSuccessful += (s, user) =>
            {
                MessageBox.Show("Регистрация успешна! Теперь вы можете войти.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            };
            vm.CancelRequested += (s, e) =>
            {
                DialogResult = false;
                Close();
            };
            vm.LoginRequested += (s, e) =>
            {
                DialogResult = false;
                Close();
            };
            DataContext = vm;
        }
    }
}