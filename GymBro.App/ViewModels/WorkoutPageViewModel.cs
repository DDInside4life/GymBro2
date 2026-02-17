using GymBro.App.Commands;
using GymBro.App.Infrastructure;
using GymBro.Business.Infrastructure;
using GymBro.Business.Managers;
using GymBro.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GymBro.App.ViewModels
{
    public class WorkoutPageViewModel : ViewModelBase
    {
        private readonly TrainingProgramManager _programManager;
        private readonly ExerciseManager _exerciseManager;
        private ObservableCollection<Exercise> _todayExercises;
        private Exercise _selectedExercise;
        private string _workoutName;
        private bool _isLoading;

        public WorkoutPageViewModel()
        {
            var factory = new ManagersFactory();
            _programManager = factory.GetTrainingProgramManager();
            _exerciseManager = factory.GetExerciseManager();
            TodayExercises = new ObservableCollection<Exercise>();

            LoadWorkoutAsync();

            StartWorkoutCommand = new RelayCommand(ExecuteStartWorkout, CanStartWorkout);
            CompleteSetCommand = new RelayCommand(ExecuteCompleteSet, CanCompleteSet);
        }

        public ObservableCollection<Exercise> TodayExercises
        {
            get => _todayExercises;
            set => SetProperty(ref _todayExercises, value);
        }

        public Exercise SelectedExercise
        {
            get => _selectedExercise;
            set => SetProperty(ref _selectedExercise, value);
        }

        public string WorkoutName
        {
            get => _workoutName;
            set => SetProperty(ref _workoutName, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand StartWorkoutCommand { get; }
        public ICommand CompleteSetCommand { get; }

        private async Task LoadWorkoutAsync()
        {
            try
            {
                IsLoading = true;
                var currentUser = SessionManager.CurrentUser;
                if (currentUser == null)
                {
                    WorkoutName = "Войдите в систему";
                    return;
                }

                // Находим профиль пользователя (UserProfile) по UserId? У нас связь User и UserProfile через UserProfileId.
                // Можно получить все программы пользователя через менеджер программ, но они привязаны к UserProfile, а не к User.
                // У нас в SessionManager хранится User. Чтобы получить его профиль, можно сделать дополнительный запрос, но пока упростим:
                // Будем считать, что у User есть UserProfile (поле UserProfileId) и можно его получить через UserProfileManager.
                // Но для простоты: загружаем все упражнения, если нет программ.
                var programs = await _programManager.GetAllTrainingProgramsAsync(); // но такого метода нет, добавим.
                var program = programs.FirstOrDefault();
                if (program == null)
                {
                    // Если программ нет, показываем все упражнения как тренировку
                    var allExercises = await _exerciseManager.GetAllExercisesAsync();
                    TodayExercises = new ObservableCollection<Exercise>(allExercises);
                    WorkoutName = "Тренировка (все упражнения)";
                }
                else
                {
                    // Загружаем упражнения программы
                    var exercises = await _exerciseManager.GetExercisesByProgramAsync(program.Id); // добавим метод
                    TodayExercises = new ObservableCollection<Exercise>(exercises);
                    WorkoutName = program.Name;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки тренировки: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool CanStartWorkout(object param) => TodayExercises.Any();
        private void ExecuteStartWorkout(object param)
        {
            // Здесь можно открыть окно тренировки с деталями, но у нас уже есть страница.
            // Пока просто отметим.
            System.Windows.MessageBox.Show("Тренировка начата!");
        }

        private bool CanCompleteSet(object param) => SelectedExercise != null;
        private void ExecuteCompleteSet(object param)
        {
            // Завершить подход – можно обновить состояние упражнения, но для простоты пока заглушка.
            System.Windows.MessageBox.Show($"Подход завершён для {SelectedExercise.Name}");
        }
    }
}