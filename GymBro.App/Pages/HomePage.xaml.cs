using GymBro.App.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace GymBro.App.Pages
{
    public partial class HomePage : Page
    {
        public event RoutedEventHandler StartWorkoutClicked;
        public event RoutedEventHandler ViewProgramClicked;
        public event RoutedEventHandler ViewProgressClicked;

        public HomePage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await ((HomePageViewModel)DataContext).RefreshDataAsync();
            DataContext = new HomePageViewModel();
        }

        private void StartWorkoutButton_Click(object sender, RoutedEventArgs e) => StartWorkoutClicked?.Invoke(this, e);
        private void ViewProgramButton_Click(object sender, RoutedEventArgs e) => ViewProgramClicked?.Invoke(this, e);
        private void ViewProgressButton_Click(object sender, RoutedEventArgs e) => ViewProgressClicked?.Invoke(this, e);
    }
}