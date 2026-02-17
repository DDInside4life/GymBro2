using GymBro.App.Commands;
using GymBro.Business.Infrastructure;
using GymBro.Business.Managers;
using GymBro.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows;

namespace GymBro.App.ViewModels
{
    public class EditTrainingProgramViewModel : ViewModelBase
    {
        private readonly TrainingProgramManager _programManager;
        private readonly ExerciseManager _exerciseManager;
        private TrainingProgram _originalProgram;
        private string _name;
        private string _description;
        private string _programType;
        private int _durationWeeks;
        private int _difficulty;
        private int _workoutsPerWeek;
        private ObservableCollection<Exercise> _exercises;


        public EditTrainingProgramViewModel(TrainingProgram program = null)
        {
            var factory = new ManagersFactory();
            _programManager = factory.GetTrainingProgramManager();
            _exerciseManager = factory.GetExerciseManager();

            _originalProgram = program;
            if (program != null)
            {
                _name = program.Name;
                _description = program.Description;
                _programType = program.ProgramType;
                _durationWeeks = program.DurationWeeks;
                _difficulty = program.Difficulty;
                _workoutsPerWeek = program.WorkoutsPerWeek;
                LoadExercises(program.Id);
            }
            else
            {
                _durationWeeks = 8;
                _difficulty = 3;
                _workoutsPerWeek = 3;
                _programType = "Силовая";
                _exercises = new ObservableCollection<Exercise>();
            }

            OkCommand = new RelayCommand(ExecuteOk, CanExecuteOk);
            CancelCommand = new RelayCommand(ExecuteCancel);
            AddExerciseCommand = new RelayCommand(ExecuteAddExercise);
            RemoveExerciseCommand = new RelayCommand(ExecuteRemoveExercise, CanRemoveExercise);


        }


        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public string Description { get => _description; set => SetProperty(ref _description, value); }
        public string ProgramType { get => _programType; set => SetProperty(ref _programType, value); }
        public int DurationWeeks { get => _durationWeeks; set => SetProperty(ref _durationWeeks, value); }
        public int Difficulty { get => _difficulty; set => SetProperty(ref _difficulty, value); }
        public int WorkoutsPerWeek { get => _workoutsPerWeek; set => SetProperty(ref _workoutsPerWeek, value); }
        public ObservableCollection<Exercise> Exercises { get => _exercises; set => SetProperty(ref _exercises, value); }
        public List<string> ProgramTypes { get; } = new List<string> { "Силовая", "Кардио", "Фулбоди", "Сплит", "Набор массы", "Похудение" };

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand AddExerciseCommand { get; }
        public ICommand RemoveExerciseCommand { get; }

        private bool CanExecuteOk(object param)
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(ProgramType) &&
                   DurationWeeks > 0 &&
                   Difficulty >= 1 && Difficulty <= 5 &&
                   WorkoutsPerWeek >= 1 && WorkoutsPerWeek <= 7;
        }

        private void ExecuteOk(object param)
        {
            if (_originalProgram == null)
            {
                // Новый объект будет создан в окне
                var program = new TrainingProgram
                {
                    Name = Name,
                    Description = Description,
                    ProgramType = ProgramType,
                    DurationWeeks = DurationWeeks,
                    Difficulty = Difficulty,
                    WorkoutsPerWeek = WorkoutsPerWeek,
                    CreatedDate = DateTime.Now,
                    Exercises = Exercises.ToList()
                };
                CloseRequested?.Invoke(this, program);
            }
            else
            {
                // Обновляем существующий
                _originalProgram.Name = Name;
                _originalProgram.Description = Description;
                _originalProgram.ProgramType = ProgramType;
                _originalProgram.DurationWeeks = DurationWeeks;
                _originalProgram.Difficulty = Difficulty;
                _originalProgram.WorkoutsPerWeek = WorkoutsPerWeek;
                // Обновление упражнений потребует дополнительной логики, пока оставим как есть
                CloseRequested?.Invoke(this, _originalProgram);
            }
        }

        private void ExecuteCancel(object param) => CloseRequested?.Invoke(this, null);

        private void LoadExercises(int programId)
        {
            // Используем синхронный метод менеджера
            var exercises = _exerciseManager.GetExercisesByProgram(programId);
            Exercises = new ObservableCollection<Exercise>(exercises);
        }

        private void ExecuteAddExercise(object param)
        {
            var selected = Views.SelectExerciseWindow.ShowDialog(Application.Current.MainWindow);
            if (selected != null)
            {
                Exercises.Add(selected);
            }
        }

        private bool CanRemoveExercise(object param) => param is Exercise;
        private void ExecuteRemoveExercise(object param)
        {
            if (param is Exercise ex)
                Exercises.Remove(ex);
        }

        public event EventHandler<TrainingProgram> CloseRequested;


    }
}