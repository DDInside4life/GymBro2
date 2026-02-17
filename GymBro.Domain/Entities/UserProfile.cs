using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GymBro.Domain.Entities
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal Weight { get; set; } // кг
        public decimal Height { get; set; } // см
        public DateTime BirthDate { get; set; }
        public string FitnessLevel { get; set; }
        public string TrainingGoal { get; set; }

        //public Gender Gender { get; set; }
        //public FitnessLevel FitnessLevel { get; set; }
        //public TrainingGoal PrimaryGoal { get; set; }
        //public List<TrainingGoal> SecondaryGoals { get; set; }
        //public List<string> Injuries { get; set; } // Травмы
        //public List<string> EquipmentAvailable { get; set; } // Доступное оборудование
        //public int DaysPerWeek { get; set; } // Дней тренировок в неделю
        //public TimeSpan AvailableTimePerDay { get; set; }
        //public DateTime CreatedDate { get; set; }

        // Навигационные свойства
        public ICollection<TrainingProgram> TrainingPrograms { get; set; }
        //public ICollection<WorkoutSession> WorkoutSessions { get; set; }

        public UserProfile()
        {
            TrainingPrograms = [];
        }
    }
}
