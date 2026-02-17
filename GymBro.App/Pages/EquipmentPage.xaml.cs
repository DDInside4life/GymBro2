using GymBro.App.ViewModels;
using System.Windows.Controls;

namespace GymBro.App.Pages
{
    public partial class EquipmentPage : Page
    {
        public EquipmentPage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await ((EquipmentPageViewModel)DataContext).LoadEquipmentAsync();
        }
    }
}