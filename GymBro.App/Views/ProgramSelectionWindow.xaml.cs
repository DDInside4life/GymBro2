using GymBro.App.ViewModels;
using GymBro.Domain.Entities;
using System.Windows;

namespace GymBro.App.Views
{
    public partial class ProgramSelectionWindow : Window
    {
        public ProgramSelectionWindow()
        {
            InitializeComponent();
            var vm = new ProgramSelectionViewModel();
            vm.CloseRequested += (s, template) => { SelectedTemplate = template; DialogResult = template != null; Close(); };
            DataContext = vm;
        }

        public TrainingProgram SelectedTemplate { get; private set; }

        public static TrainingProgram ShowDialog(Window owner = null)
        {
            var window = new ProgramSelectionWindow();
            window.Owner = owner;
            return ((Window)window).ShowDialog() == true ? window.SelectedTemplate : null;
        }
    }
}