using GymBro.App.Commands;
using GymBro.Domain.Entities;
using System;
using System.Windows;
using System.Windows.Input;

namespace GymBro.App.Views
{
    public partial class EditUserProfileWindow : Window
    {
        // Dependency Properties для привязки данных
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(EditUserProfileWindow));
        public string Name
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        public static readonly DependencyProperty AgeProperty =
            DependencyProperty.Register("Age", typeof(int), typeof(EditUserProfileWindow));
        public int Age
        {
            get => (int)GetValue(AgeProperty);
            set => SetValue(AgeProperty, value);
        }

        public static readonly DependencyProperty WeightProperty =
            DependencyProperty.Register("Weight", typeof(decimal), typeof(EditUserProfileWindow));
        public decimal Weight
        {
            get => (decimal)GetValue(WeightProperty);
            set => SetValue(WeightProperty, value);
        }

        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.Register("Height", typeof(decimal), typeof(EditUserProfileWindow));
        public decimal Height
        {
            get => (decimal)GetValue(HeightProperty);
            set => SetValue(HeightProperty, value);
        }

        public static readonly DependencyProperty BirthDateProperty =
            DependencyProperty.Register("BirthDate", typeof(DateTime), typeof(EditUserProfileWindow));
        public DateTime BirthDate
        {
            get => (DateTime)GetValue(BirthDateProperty);
            set => SetValue(BirthDateProperty, value);
        }

        public static readonly DependencyProperty FitnessLevelProperty =
            DependencyProperty.Register("FitnessLevel", typeof(string), typeof(EditUserProfileWindow));
        public string FitnessLevel
        {
            get => (string)GetValue(FitnessLevelProperty);
            set => SetValue(FitnessLevelProperty, value);
        }

        public static readonly DependencyProperty TrainingGoalProperty =
            DependencyProperty.Register("TrainingGoal", typeof(string), typeof(EditUserProfileWindow));
        public string TrainingGoal
        {
            get => (string)GetValue(TrainingGoalProperty);
            set => SetValue(TrainingGoalProperty, value);
        }

        // Команды
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public EditUserProfileWindow()
        {
            InitializeComponent();
            DataContext = this;

            OkCommand = new RelayCommand(ExecuteOk, CanExecuteOk);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        private bool CanExecuteOk(object param)
        {
            return !string.IsNullOrWhiteSpace(Name) && Age > 0 && Weight > 0 && Height > 0
                   && !string.IsNullOrWhiteSpace(FitnessLevel) && !string.IsNullOrWhiteSpace(TrainingGoal);
        }

        private void ExecuteOk(object param)
        {
            DialogResult = true;
            Close();
        }

        private void ExecuteCancel(object param)
        {
            DialogResult = false;
            Close();
        }

        /// <summary>
        /// Открывает окно редактирования профиля.
        /// </summary>
        /// <param name="profile">Профиль для редактирования (null для нового).</param>
        /// <param name="owner">Владелец окна.</param>
        /// <returns>Новый или обновлённый профиль, или null, если пользователь отменил действие.</returns>
        public static UserProfile ShowDialog(UserProfile profile = null, Window owner = null)
        {
            var window = new EditUserProfileWindow
            {
                Owner = owner
            };

            if (profile != null)
            {
                // Редактирование существующего профиля
                window.Name = profile.Name;
                window.Age = profile.Age;
                window.Weight = profile.Weight;
                window.Height = profile.Height;
                window.BirthDate = profile.BirthDate;
                window.FitnessLevel = profile.FitnessLevel;
                window.TrainingGoal = profile.TrainingGoal;
            }
            else
            {
                // Новый профиль – задаём значения по умолчанию
                window.Age = 25;
                window.Weight = 70;
                window.Height = 170;
                window.BirthDate = DateTime.Today.AddYears(-25);
                window.FitnessLevel = "Средний";
                window.TrainingGoal = "Поддержание";
            }

            if (((Window)window).ShowDialog() == true)
            {
                return new UserProfile
                {
                    Name = window.Name,
                    Age = window.Age,
                    Weight = window.Weight,
                    Height = window.Height,
                    BirthDate = window.BirthDate,
                    FitnessLevel = window.FitnessLevel,
                    TrainingGoal = window.TrainingGoal
                };
            }
            return null;
        }
    }
}