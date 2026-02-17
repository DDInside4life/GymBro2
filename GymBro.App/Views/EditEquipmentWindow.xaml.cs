using GymBro.Domain.Entities;
using System.Windows;

namespace GymBro.App.Views
{
    public partial class EditEquipmentWindow : Window
    {
        public EditEquipmentWindow(Equipment equipment = null)
        {
            InitializeComponent();
            if (equipment != null)
            {
                NameTextBox.Text = equipment.Name;
                DescriptionTextBox.Text = equipment.Description;
            }
        }

        public string EquipmentName => NameTextBox.Text.Trim();
        public string EquipmentDescription => DescriptionTextBox.Text.Trim();

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EquipmentName))
            {
                MessageBox.Show("Введите название оборудования");
                return;
            }
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}