using GymBro.App.Commands;
using GymBro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace GymBro.App.ViewModels
{
    public class EditUserProfileViewModel : ViewModelBase
    {
        private string _name;
        private int _age;
        private decimal _weight;
        private decimal _height;
        private DateTime _birthDate = DateTime.Today;
        private string _fitnessLevel;
        private string _trainingGoal;
        private readonly List<string> _fitnessLevels = new() { "Начинающий", "Средний", "Продвинутый" };
        private readonly List<string> _trainingGoals = new() { "Похудение", "Набор массы", "Поддержание" };

        public EditUserProfileViewModel(UserProfile user = null)
        {
            if (user != null)
            {
                _name = user.Name;
                _age = user.Age;
                _weight = user.Weight;
                _height = user.Height;
                _birthDate = user.BirthDate;
                _fitnessLevel = user.FitnessLevel;
                _trainingGoal = user.TrainingGoal;
            }
            else
            {
                _age = 25;
                _weight = 70;
                _height = 170;
                _fitnessLevel = "Средний";
                _trainingGoal = "Поддержание";
            }
            OkCommand = new RelayCommand(ExecuteOk, CanOk);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public int Age { get => _age; set => SetProperty(ref _age, value); }
        public decimal Weight { get => _weight; set => SetProperty(ref _weight, value); }
        public decimal Height { get => _height; set => SetProperty(ref _height, value); }
        public DateTime BirthDate { get => _birthDate; set => SetProperty(ref _birthDate, value); }
        public string FitnessLevel { get => _fitnessLevel; set => SetProperty(ref _fitnessLevel, value); }
        public string TrainingGoal { get => _trainingGoal; set => SetProperty(ref _trainingGoal, value); }
        public List<string> FitnessLevels => _fitnessLevels;
        public List<string> TrainingGoals => _trainingGoals;

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        private bool CanOk(object param) =>
            !string.IsNullOrWhiteSpace(Name) && Age > 0 && Weight > 0 && Height > 0;

        private void ExecuteOk(object param) => CloseRequested?.Invoke(this, true);
        private void ExecuteCancel(object param) => CloseRequested?.Invoke(this, false);

        public event EventHandler<bool> CloseRequested;
    }
}