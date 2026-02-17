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

            LoadUsersAsync();

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

        private async Task LoadUsersAsync()
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

        private void ExecuteAddUser(object param)
        {
            // TODO: открыть окно добавления пользователя (можно использовать RegisterWindow, но с ролью)
            MessageBox.Show("Добавление пользователя будет реализовано позже.");
        }

        private void ExecuteEditUser(object param)
        {
            if (SelectedUser == null) return;
            // TODO: редактирование пользователя (изменение роли, языка и т.д.)
            MessageBox.Show("Редактирование пользователя будет реализовано позже.");
        }

        private async void ExecuteDeleteUser(object param)
        {
            if (SelectedUser == null) return;
            if (SelectedUser.Id == SessionManager.CurrentUser?.Id)
            {
                MessageBox.Show("Нельзя удалить самого себя.");
                return;
            }
            var result = MessageBox.Show($"Удалить пользователя '{SelectedUser.Login}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // TODO: реализовать удаление пользователя (с каскадным удалением профиля?)
                MessageBox.Show("Удаление временно отключено.");
            }
        }
    }
}