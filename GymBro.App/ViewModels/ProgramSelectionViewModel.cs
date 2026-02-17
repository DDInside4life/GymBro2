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
    public class ProgramSelectionViewModel : ViewModelBase
    {
        private readonly TrainingProgramManager _programManager;
        private ObservableCollection<TrainingProgram> _templates;
        private TrainingProgram _selectedTemplate;

        public ProgramSelectionViewModel()
        {
            var factory = new ManagersFactory();
            _programManager = factory.GetTrainingProgramManager();
            LoadTemplates();

            SelectCommand = new RelayCommand(ExecuteSelect, CanExecuteSelect);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        public ObservableCollection<TrainingProgram> Templates
        {
            get => _templates;
            set => SetProperty(ref _templates, value);
        }

        public TrainingProgram SelectedTemplate
        {
            get => _selectedTemplate;
            set => SetProperty(ref _selectedTemplate, value);
        }

        public ICommand SelectCommand { get; }
        public ICommand CancelCommand { get; }

        private async void LoadTemplates()
        {
            var templates = await _programManager.GetAllTemplatesAsync(); // нужен новый метод
            Templates = new ObservableCollection<TrainingProgram>(templates);
        }

        private bool CanExecuteSelect(object param) => SelectedTemplate != null;
        private void ExecuteSelect(object param) => CloseRequested?.Invoke(this, SelectedTemplate);
        private void ExecuteCancel(object param) => CloseRequested?.Invoke(this, null);

        public event EventHandler<TrainingProgram> CloseRequested;
    }
}