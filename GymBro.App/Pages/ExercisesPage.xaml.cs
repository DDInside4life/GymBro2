using GymBro.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GymBro.App.Pages
{
    /// <summary>
    /// Interaction logic for ExercisesPage.xaml
    /// </summary>
    public partial class ExercisesPage : Page
{
    public ExercisesPage()
    {
        InitializeComponent();
        Loaded += async (s, e) => await ((ExercisesPageViewModel)DataContext).LoadExercisesAsync();
    }
}
}
