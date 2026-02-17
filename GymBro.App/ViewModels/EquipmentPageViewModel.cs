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
    public class EquipmentPageViewModel : ViewModelBase
    {
        private readonly EquipmentManager _equipmentManager;
        private ObservableCollection<Equipment> _equipmentList;
        private Equipment _selectedEquipment;
        private ObservableCollection<Exercise> _exercisesForSelected;
        private bool _isLoading;
        private readonly bool _isAdmin;
        private Task _currentLoadTask;

        public EquipmentPageViewModel()
        {
            var factory = new ManagersFactory();
            _equipmentManager = factory.GetEquipmentManager();
            _isAdmin = SessionManager.IsInRole("Admin");

            EquipmentList = new ObservableCollection<Equipment>();
            ExercisesForSelected = new ObservableCollection<Exercise>();

            //LoadEquipmentAsync();

            AddEquipmentCommand = new RelayCommand(ExecuteAddEquipment, _ => _isAdmin);
            EditEquipmentCommand = new RelayCommand(ExecuteEditEquipment, _ => _isAdmin && SelectedEquipment != null);
            DeleteEquipmentCommand = new RelayCommand(ExecuteDeleteEquipment, _ => _isAdmin && SelectedEquipment != null);
        }

        public ObservableCollection<Equipment> EquipmentList
        {
            get => _equipmentList;
            set => SetProperty(ref _equipmentList, value);
        }

        public Equipment SelectedEquipment
        {
            get => _selectedEquipment;
            set
            {
                if (SetProperty(ref _selectedEquipment, value))
                {
                    // Запускаем загрузку, но не ожидаем (fire-and-forget) – плохо, лучше сделать асинхронную команду.
                    // Временно можно так, но с проверкой IsLoading.
                    _ = LoadExercisesForEquipmentAsync(value?.Id);
                }
            }
        }

        public ObservableCollection<Exercise> ExercisesForSelected
        {
            get => _exercisesForSelected;
            set => SetProperty(ref _exercisesForSelected, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool IsAdmin => _isAdmin;

        public ICommand AddEquipmentCommand { get; }
        public ICommand EditEquipmentCommand { get; }
        public ICommand DeleteEquipmentCommand { get; }

        public async Task LoadEquipmentAsync()
        {
            if (IsLoading) return; // уже загружается
            try
            {
                IsLoading = true;
                var equipment = await _equipmentManager.GetAllEquipmentAsync();
                EquipmentList.Clear();
                foreach (var eq in equipment) EquipmentList.Add(eq);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadExercisesForEquipmentAsync(int? equipmentId)
        {
            if (IsLoading) return; // если идёт загрузка, пропускаем (или можно ожидать)
            if (equipmentId == null)
            {
                ExercisesForSelected.Clear();
                return;
            }

            try
            {
                IsLoading = true;
                var exercises = await _equipmentManager.GetExercisesForEquipmentAsync(equipmentId.Value);
                ExercisesForSelected.Clear();
                foreach (var ex in exercises) ExercisesForSelected.Add(ex);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ExecuteAddEquipment(object param)
        {
            var dialog = new Views.EditEquipmentWindow();
            dialog.Owner = Application.Current.MainWindow;
            if (dialog.ShowDialog() == true)
            {
                var newEquipment = new Equipment
                {
                    Name = dialog.EquipmentName,
                    Description = dialog.EquipmentDescription
                };
                _equipmentManager.CreateEquipmentAsync(newEquipment).ContinueWith(_ =>
                {
                    Application.Current.Dispatcher.Invoke(() => LoadEquipmentAsync());
                });
            }
        }

        private void ExecuteEditEquipment(object param)
        {
            if (SelectedEquipment == null) return;
            var dialog = new Views.EditEquipmentWindow(SelectedEquipment);
            dialog.Owner = Application.Current.MainWindow;
            if (dialog.ShowDialog() == true)
            {
                SelectedEquipment.Name = dialog.EquipmentName;
                SelectedEquipment.Description = dialog.EquipmentDescription;
                _equipmentManager.UpdateEquipmentAsync(SelectedEquipment).ContinueWith(_ =>
                {
                    Application.Current.Dispatcher.Invoke(() => LoadEquipmentAsync());
                });
            }
        }

        private async void ExecuteDeleteEquipment(object param)
        {
            if (SelectedEquipment == null) return;
            var result = MessageBox.Show($"Удалить оборудование '{SelectedEquipment.Name}'?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (await _equipmentManager.DeleteEquipmentAsync(SelectedEquipment.Id))
                {
                    await LoadEquipmentAsync();
                }
            }
        }
    }
}