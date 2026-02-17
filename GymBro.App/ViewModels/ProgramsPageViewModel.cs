using GymBro.App;
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
    public class ProgramsPageViewModel : ViewModelBase
    {
        private readonly TrainingProgramManager _programManager;
        private readonly ExerciseManager _exerciseManager;
        private ObservableCollection<TrainingProgram> _programs;
        private TrainingProgram _selectedProgram;
        private bool _isLoading;
        private readonly bool _isAdmin;
        private readonly int? _currentUserProfileId;

        public ProgramsPageViewModel()
        {
            var factory = new ManagersFactory();
            _programManager = factory.GetTrainingProgramManager();
            _exerciseManager = factory.GetExerciseManager();

            _isAdmin = SessionManager.IsInRole("Admin");
            _currentUserProfileId = SessionManager.CurrentUser?.UserProfileId;

            Programs = new ObservableCollection<TrainingProgram>();

            // Загружаем программы при создании
            //Task.Run(() => LoadProgramsAsync()).ContinueWith(t =>
            //{
            //    if (t.IsFaulted)
            //    {
            //        MessageBox.Show($"Ошибка загрузки: {t.Exception?.InnerException?.Message}");
            //    }
            //});

            SelectProgramCommand = new RelayCommand(ExecuteSelectProgram, CanSelectProgram);
            CreateProgramCommand = new RelayCommand(ExecuteCreateProgram);
            EditProgramCommand = new RelayCommand(ExecuteEditProgram, CanEditProgram);
            DeleteProgramCommand = new RelayCommand(ExecuteDeleteProgram, CanDeleteProgram);
        }

        public ObservableCollection<TrainingProgram> Programs
        {
            get => _programs;
            set => SetProperty(ref _programs, value);
        }

        public TrainingProgram SelectedProgram
        {
            get => _selectedProgram;
            set => SetProperty(ref _selectedProgram, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool IsAdmin => _isAdmin;

        public ICommand SelectProgramCommand { get; }
        public ICommand CreateProgramCommand { get; }
        public ICommand EditProgramCommand { get; }
        public ICommand DeleteProgramCommand { get; }

        public async Task LoadProgramsAsync()
        {
            try
            {
                IsLoading = true;
                var programs = await _programManager.GetProgramsForCurrentUserAsync(_currentUserProfileId, _isAdmin);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Programs.Clear();
                    foreach (var prog in programs)
                    {
                        Programs.Add(prog);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки программ: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool CanSelectProgram(object param) => SelectedProgram != null;

        private void ExecuteSelectProgram(object param)
        {
            SessionManager.SelectedProgramId = SelectedProgram?.Id;
            if (Application.Current.MainWindow is MainWindow mainWindow
                 && mainWindow.DataContext is MainWindowViewModel viewModel)
            {
                viewModel.NavigateCommand.Execute("Home");
            }
        }

        private void ExecuteCreateProgram(object param)
        {
            var newProgram = Views.EditTrainingProgramWindow.ShowDialog(owner: Application.Current.MainWindow);
            if (newProgram != null)
            {
                newProgram.UserProfileId = _currentUserProfileId ?? 0;
                newProgram.IsTemplate = false;
                _programManager.CreateTrainingProgram(newProgram);
                Programs.Add(newProgram);
                SelectedProgram = newProgram;
            }
        }

        private bool CanEditProgram(object param)
        {
            if (SelectedProgram == null) return false;
            if (_isAdmin) return true;
            return SelectedProgram.UserProfileId == _currentUserProfileId && !SelectedProgram.IsTemplate;
        }

        private void ExecuteEditProgram(object param)
        {
            var updatedProgram = Views.EditTrainingProgramWindow.ShowDialog(SelectedProgram, Application.Current.MainWindow);
            if (updatedProgram != null)
            {
                updatedProgram.Id = SelectedProgram.Id;
                updatedProgram.UserProfileId = SelectedProgram.UserProfileId;
                _programManager.UpdateTrainingProgram(updatedProgram);
                var index = Programs.IndexOf(SelectedProgram);
                Programs[index] = updatedProgram;
                SelectedProgram = updatedProgram;
            }
        }

        private bool CanDeleteProgram(object param) => _isAdmin && SelectedProgram != null;

        private void ExecuteDeleteProgram(object param)
        {
            var result = MessageBox.Show($"Удалить программу '{SelectedProgram.Name}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (_programManager.DeleteTrainingProgram(SelectedProgram.Id))
                {
                    Programs.Remove(SelectedProgram);
                    SelectedProgram = Programs.FirstOrDefault();
                }
                else
                {
                    MessageBox.Show("Не удалось удалить программу.");
                }
            }
        }
    }
}