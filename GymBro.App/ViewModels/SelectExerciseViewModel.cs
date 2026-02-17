using GymBro.App.Commands;
using GymBro.Business.Infrastructure;
using GymBro.Business.Managers;
using GymBro.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace GymBro.App.ViewModels
{
    public class SelectExerciseViewModel : ViewModelBase
    {
        private readonly ExerciseManager _exerciseManager;
        private ObservableCollection<Exercise> _exercises;
        private Exercise _selectedExercise;

        public SelectExerciseViewModel()
        {
            var factory = new ManagersFactory();
            _exerciseManager = factory.GetExerciseManager();
            LoadExercises();

            AddCommand = new RelayCommand(ExecuteAdd, CanExecuteAdd);
            CancelCommand = new RelayCommand(ExecuteCancel);
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

        public ICommand AddCommand { get; }
        public ICommand CancelCommand { get; }

        private async void LoadExercises()
        {
            var exercises = await _exerciseManager.GetAllExercisesAsync();
            Exercises = new ObservableCollection<Exercise>(exercises);
        }

        private bool CanExecuteAdd(object param) => SelectedExercise != null;
        private void ExecuteAdd(object param) => CloseRequested?.Invoke(this, SelectedExercise);
        private void ExecuteCancel(object param) => CloseRequested?.Invoke(this, null);

        public event EventHandler<Exercise> CloseRequested;
    }
}