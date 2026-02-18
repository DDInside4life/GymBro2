using GymBro.App.Commands;
using GymBro.App.Infrastructure;
using GymBro.Business.Infrastructure;
using GymBro.Business.Managers;
using GymBro.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GymBro.App.ViewModels
{
    public class UsersPageViewModel : ViewModelBase
    {
        private readonly UserManager _userManager;
        private ObservableCollection<User> _users;
        private User _selectedUser;
        private bool _isLoading;
        private readonly bool _isAdmin;

        public UsersPageViewModel()
        {
            var factory = new ManagersFactory();
            _userManager = factory.GetUserManager();
            _isAdmin = SessionManager.IsInRole("Admin");

            Users = new ObservableCollection<User>();



            AddUserCommand = new RelayCommand(ExecuteAddUser, _ => _isAdmin);
            EditUserCommand = new RelayCommand(ExecuteEditUser, _ => _isAdmin && SelectedUser != null);
            DeleteUserCommand = new RelayCommand(ExecuteDeleteUser, _ => _isAdmin && SelectedUser != null);
        }

        public ObservableCollection<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set => SetProperty(ref _selectedUser, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool IsAdmin => _isAdmin;

        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }

        public async Task LoadUsersAsync()
        {
            try
            {
                IsLoading = true;
                // Получаем всех пользователей (можно через UserManager)
                var users = await _userManager.GetAllUsersAsync(); // нужно добавить этот метод
                Users.Clear();
                foreach (var user in users) Users.Add(user);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void ExecuteAddUser(object param)
        {
            var allRoles = (await _userManager.GetAllRolesAsync()).ToList();
            var dialog = new Views.EditUserWindow(null, allRoles);
            dialog.Owner = Application.Current.MainWindow;
            if (dialog.ShowDialog() == true)
            {
                var newUser = new User
                {
                    Login = dialog.Login,
                    FullName = dialog.FullName,
                    Email = dialog.Email,
                    Roles = new List<Role> { dialog.SelectedRole }
                };
                try
                {
                    await _userManager.CreateUserAsync(newUser, dialog.Password);
                    await LoadUsersAsync(); // перезагрузить список
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        private async void ExecuteEditUser(object param)
        {
            if (SelectedUser == null) return;
            var allRoles = (await _userManager.GetAllRolesAsync()).ToList();
            var dialog = new Views.EditUserWindow(SelectedUser, allRoles);
            dialog.Owner = Application.Current.MainWindow;
            if (dialog.ShowDialog() == true)
            {
                SelectedUser.Login = dialog.Login;
                SelectedUser.FullName = dialog.FullName;
                SelectedUser.Email = dialog.Email;
                SelectedUser.Roles = new List<Role> { dialog.SelectedRole };
                try
                {
                    await _userManager.UpdateUserAsync(SelectedUser, dialog.Password); // пароль может быть пустым
                    await LoadUsersAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        private async void ExecuteDeleteUser(object param)
        {
            var userToDelete = SelectedUser;
            if (userToDelete == null) return;

            if (userToDelete.Id == SessionManager.CurrentUser?.Id)
            {
                MessageBox.Show("Нельзя удалить самого себя.");
                return;
            }
            var login = string.IsNullOrWhiteSpace(userToDelete.Login) ? $"ID={userToDelete.Id}" : userToDelete.Login;
            var result = MessageBox.Show($"Удалить пользователя '{login}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            try
            {
                IsLoading = true;
                var deleted = await _userManager.DeleteUserAsync(userToDelete.Id);
                if (!deleted)
                {
                    MessageBox.Show("Пользователь не найден или уже удалён.");
                    return;
                }

                await LoadUsersAsync();
                SelectedUser = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления пользователя: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
                {
                    if (await _userManager.DeleteUserAsync(SelectedUser.Id))
                    {
                        Users.Remove(SelectedUser);
                        SelectedUser = Users.FirstOrDefault();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить пользователя.");
                    }
                }
            }
        }
    }
}