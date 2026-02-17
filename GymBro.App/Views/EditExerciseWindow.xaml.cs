using GymBro.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GymBro.App.Views
{
    public partial class EditExerciseWindow : Window
    {
        public EditExerciseWindow(Exercise exercise = null, List<Equipment> allEquipment = null)
        {
            InitializeComponent();
            if (exercise != null)
            {
                NameTextBox.Text = exercise.Name;
                DescriptionTextBox.Text = exercise.Description;
                SetsTextBox.Text = exercise.DefaultSets.ToString();
                RepsMinTextBox.Text = exercise.DefaultRepsMin.ToString();
                RepsMaxTextBox.Text = exercise.DefaultRepsMax.ToString();
                RestSecondsTextBox.Text = ((int)exercise.RestBetweenSets.TotalSeconds).ToString();
                TechniqueTipsTextBox.Text = exercise.TechniqueTips;
                CommonMistakesTextBox.Text = exercise.CommonMistakes;
                // Выбор оборудования
                if (allEquipment != null)
                {
                    EquipmentListBox.ItemsSource = allEquipment;
                    var selectedIds = exercise.Equipment?.Select(e => e.Id).ToList() ?? new List<int>();
                    foreach (var item in EquipmentListBox.Items)
                    {
                        if (item is Equipment eq && selectedIds.Contains(eq.Id))
                            EquipmentListBox.SelectedItems.Add(eq);
                    }
                }
            }
            else
            {
                EquipmentListBox.ItemsSource = allEquipment;
            }
        }

        public string ExerciseName => NameTextBox.Text.Trim();
        public string ExerciseDescription => DescriptionTextBox.Text.Trim();
        public int DefaultSets => int.TryParse(SetsTextBox.Text, out var s) ? s : 0;
        public int RepsMin => int.TryParse(RepsMinTextBox.Text, out var r) ? r : 0;
        public int RepsMax => int.TryParse(RepsMaxTextBox.Text, out var r) ? r : 0;
        public int RestSeconds => int.TryParse(RestSecondsTextBox.Text, out var rs) ? rs : 60;
        public string TechniqueTips => TechniqueTipsTextBox.Text.Trim();
        public string CommonMistakes => CommonMistakesTextBox.Text.Trim();
        public List<Equipment> SelectedEquipment => EquipmentListBox.SelectedItems.Cast<Equipment>().ToList();

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ExerciseName))
            {
                MessageBox.Show("Введите название упражнения");
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