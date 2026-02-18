using GymBro.App.ViewModels;
using GymBro.Domain.Entities;
using System.Windows;
using System.Windows.Controls;
using GymBro.App.Views;

namespace GymBro.App
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();

                if (DataContext is MainWindowViewModel vm)
                {
                    vm.NavigationRequested += OnNavigationRequested;
                    vm.NavigateCommand.Execute("Home");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка в MainWindow: {ex.Message}\n\n{ex.StackTrace}");
                throw;
            }
        }


        private void OnNavigationRequested(object sender, string pageKey)
        {
            // Загружаем соответствующую страницу по ключу
            switch (pageKey)
            {
                case "Home":
                    var homePage = new Pages.HomePage();

                    // Подписка на события HomePage
                    homePage.ViewProgramClicked += (s, e) =>
                    {
                        var programWindow = new ProgramSelectionWindow(); // откроется окно выбора программы
                        programWindow.Owner = this;
                        ((Window)programWindow).ShowDialog();
                    };
                    MainContentFrame.Navigate(homePage);
                    break;
                case "Programs":
                    MainContentFrame.Navigate(new Pages.ProgramsPage());
                    break;
                case "Exercises":
                    MainContentFrame.Navigate(new Pages.ExercisesPage());
                    break;
                case "Equipment":
                    MainContentFrame.Navigate(new Pages.EquipmentPage());
                    break;
                case "Users":
                    var usersPage = new Pages.UsersPage
                    {
                        DataContext = new UsersPageViewModel()
                    };
                    MainContentFrame.Navigate(usersPage);
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}