using GymBro.App.ViewModels;
using GymBro.Domain.Entities;
using System.Windows;

namespace GymBro.App.Views
{
    public partial class EditTrainingProgramWindow : Window
    {
        public TrainingProgram Result { get; private set; }

        public EditTrainingProgramWindow(TrainingProgram program = null)
        {
            InitializeComponent();
            var vm = new EditTrainingProgramViewModel(program);
            vm.CloseRequested += (s, result) =>
            {
                Result = result;
                DialogResult = result != null;
                Close();
            };
            DataContext = vm;
        }

        public static TrainingProgram ShowDialog(TrainingProgram program = null, Window owner = null)
        {
            var window = new EditTrainingProgramWindow(program);
            window.Owner = owner;
            return ((Window)window).ShowDialog() == true ? window.Result : null;
        }
    }
}

