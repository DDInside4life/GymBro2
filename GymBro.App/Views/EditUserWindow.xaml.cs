using GymBro.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GymBro.App.Views
{
    public partial class EditUserWindow : Window
    {
        public EditUserWindow(User user = null, List<Role> allRoles = null)
        {
            InitializeComponent();
            if (allRoles != null)
                RoleComboBox.ItemsSource = allRoles;

            if (user != null)
            {
                LoginTextBox.Text = user.Login;
                FullNameTextBox.Text = user.FullName;
                EmailTextBox.Text = user.Email;
                if (user.Roles != null && user.Roles.Any())
                    RoleComboBox.SelectedItem = user.Roles.First(); // предполагаем, что у пользователя одна роль
            }
        }

        public string Login => LoginTextBox.Text.Trim();
        public string Password => PasswordBox.Password;
        public string FullName => FullNameTextBox.Text.Trim();
        public string Email => EmailTextBox.Text.Trim();
        public Role SelectedRole => RoleComboBox.SelectedItem as Role;

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Login))
            {
                MessageBox.Show("Логин не может быть пустым");
                return;
            }
            if (string.IsNullOrWhiteSpace(FullName))
            {
                MessageBox.Show("Полное имя не может быть пустым");
                return;
            }
            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("Email не может быть пустым");
                return;
            }
            if (SelectedRole == null)
            {
                MessageBox.Show("Выберите роль");
                return;
            }
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}