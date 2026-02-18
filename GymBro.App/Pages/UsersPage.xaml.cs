using GymBro.App.ViewModels;
using GymBro.App.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GymBro.App.Pages
{
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
            Loaded += UsersPage_Loaded;
        }

        private async void UsersPage_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= UsersPage_Loaded;

            if (DataContext is UsersPageViewModel vm)
            {
                await vm.LoadUsersAsync();
            }
            else
            {
                MessageBox.Show("Ошибка инициализации страницы пользователей.");
            }

        }
    }
}