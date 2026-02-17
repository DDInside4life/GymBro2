using GymBro.App.ViewModels;
using GymBro.Business.Infrastructure;
using GymBro.Domain.Entities;
using System.Windows;

namespace GymBro.App.Views
{
    public partial class SelectExerciseWindow : Window
    {
        public SelectExerciseWindow()
        {
            InitializeComponent();
            var vm = new SelectExerciseViewModel();
            vm.CloseRequested += (s, exercise) => { SelectedExercise = exercise; DialogResult = exercise != null; Close(); };
            DataContext = vm;
        }

        public Exercise SelectedExercise { get; private set; }

        public static Exercise ShowDialog(Window owner = null)
        {
            var window = new SelectExerciseWindow();
            window.Owner = owner;
            return ((Window)window).ShowDialog() == true ? window.SelectedExercise : null;
        }
    }
}