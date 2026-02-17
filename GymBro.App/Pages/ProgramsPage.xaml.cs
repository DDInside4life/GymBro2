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
    /// Interaction logic for ProgramsPage.xaml
    /// </summary>
    public partial class ProgramsPage : Page
    {
        public ProgramsPage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await ((ProgramsPageViewModel)DataContext).LoadProgramsAsync();
        }
    }
}
