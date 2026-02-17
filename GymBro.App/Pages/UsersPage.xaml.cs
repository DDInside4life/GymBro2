using GymBro.App.ViewModels;
using System.Windows.Controls;

namespace GymBro.App.Pages
{
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await ((EquipmentPageViewModel)DataContext).LoadEquipmentAsync();
        }
    }
}