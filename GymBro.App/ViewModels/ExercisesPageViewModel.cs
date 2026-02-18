using GymBro.App.Commands;
using GymBro.App.Infrastructure;
using GymBro.Business.Infrastructure;
using GymBro.Business.Managers;
using GymBro.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Threading;
using System.Windows;

namespace GymBro.App.ViewModels
{
    public class ExercisesPageViewModel : ViewModelBase
    {
        private readonly ExerciseManager _exerciseManager;
        private ObservableCollection<Exercise> _exercises;
        private Exercise _selectedExercise;
        private bool _isLoading;
        private readonly EquipmentManager _equipmentManager;
        private readonly bool _isAdmin;
        private readonly SemaphoreSlim _loadSemaphore = new SemaphoreSlim(1, 1);

        public ICommand AddExerciseCommand { get; }
        public ICommand EditExerciseCommand { get; }
        public ICommand DeleteExerciseCommand { get; }

        public ExercisesPageViewModel()
        {
            var factory = new ManagersFactory();
            _exerciseManager = factory.GetExerciseManager();
            _equipmentManager = factory.GetEquipmentManager();
            _isAdmin = SessionManager.IsInRole("Admin");
            Exercises = new ObservableCollection<Exercise>();
            LoadExercisesAsync();

            AddExerciseCommand = new RelayCommand(ExecuteAddExercise, _ => _isAdmin);
            EditExerciseCommand = new RelayCommand(ExecuteEditExercise, CanEditOrDelete);
            DeleteExerciseCommand = new RelayCommand(ExecuteDeleteExercise, CanEditOrDelete);
        }

        private bool CanEditOrDelete(object param) => _isAdmin && SelectedExercise != null;

        private async void ExecuteEditExercise(object param)
        {
            if (SelectedExercise == null) return;
            var allEquipment = (await _equipmentManager.GetAllEquipmentAsync()).ToList();
            var dialog = new Views.EditExerciseWindow(SelectedExercise, allEquipment);
            dialog.Owner = Application.Current.MainWindow;
            if (dialog.ShowDialog() == true)
            {
                SelectedExercise.Name = dialog.ExerciseName;
                SelectedExercise.Description = dialog.ExerciseDescription;
                SelectedExercise.DefaultSets = dialog.DefaultSets;
                SelectedExercise.DefaultRepsMin = dialog.RepsMin;
                SelectedExercise.DefaultRepsMax = dialog.RepsMax;
                SelectedExercise.RestBetweenSets = TimeSpan.FromSeconds(dialog.RestSeconds);
                SelectedExercise.TechniqueTips = dialog.TechniqueTips;
                SelectedExercise.CommonMistakes = dialog.CommonMistakes;
                SelectedExercise.Equipment = dialog.SelectedEquipment;

                await _exerciseManager.UpdateExerciseAsync(SelectedExercise);
                await LoadExercisesAsync(); // обновить список
            }
        }

        private async void ExecuteDeleteExercise(object param)
        {
            if (SelectedExercise == null) return;
            var result = MessageBox.Show($"Удалить упражнение '{SelectedExercise.Name}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (await _exerciseManager.DeleteExerciseAsync(SelectedExercise.Id))
                {
                    Exercises.Remove(SelectedExercise);
                    SelectedExercise = Exercises.FirstOrDefault();
                }
                else
                {
                    MessageBox.Show("Не удалось удалить упражнение.");
                }
            }
        }

        private async void ExecuteAddExercise(object param)
        {
            var allEquipment = (await _equipmentManager.GetAllEquipmentAsync()).ToList();
            var dialog = new Views.EditExerciseWindow(null, allEquipment);
            dialog.Owner = Application.Current.MainWindow;
            if (dialog.ShowDialog() == true)
            {
                var newExercise = new Exercise
                {
                    Name = dialog.ExerciseName,
                    Description = dialog.ExerciseDescription,
                    DefaultSets = dialog.DefaultSets,
                    DefaultRepsMin = dialog.RepsMin,
                    DefaultRepsMax = dialog.RepsMax,
                    RestBetweenSets = TimeSpan.FromSeconds(dialog.RestSeconds),
                    TechniqueTips = dialog.TechniqueTips,
                    CommonMistakes = dialog.CommonMistakes,
                    Equipment = dialog.SelectedEquipment
                };
                await _exerciseManager.CreateExerciseAsync(newExercise);
                await LoadExercisesAsync(); // перезагрузить список
            }
        }


        public ObservableCollection<Exercise> Exercises
        {
            get => _exercises;
            set => SetProperty(ref _exercises, value);
        }

        public Exercise SelectedExercise
        {
            get => _selectedExercise;
            set => SetProperty(ref _selectedExercise, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public async Task LoadExercisesAsync()
        {
            await _loadSemaphore.WaitAsync();
            try
            {
                IsLoading = true;
                var exercises = await _exerciseManager.GetAllExercisesAsync();
                Exercises.Clear();
                foreach (var ex in exercises)
                {
                    Exercises.Add(ex);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки упражнений: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
                _loadSemaphore.Release();
            }
        }
    }
}