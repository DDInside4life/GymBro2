using System;
using System.Collections.Generic;
using System.Text;

namespace GymBro.Domain.Entities
{
    public class TrainingProgram
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProgramType { get; set; } // Силовая, кардио, гибрид
        public int DurationWeeks { get; set; }
        public int Difficulty { get; set; }
        //public List<BodyPart> TargetBodyParts { get; set; }
        public DateTime CreatedDate { get; set; }
        //public List<string> RequiredEquipment { get; set; }
        //public int CaloriesPerSession { get; set; }
        //public bool IsActive { get; set; }

        public int WorkoutsPerWeek { get; set; }
        public bool IsTemplate { get; set; }


        //  Внешний ключ для связи с UserProfile
        public int UserProfileId { get; set; }

        // Навигационные свойства
        public UserProfile UserProfile { get; set; }
        public ICollection<Exercise> Exercises { get; set; }
        //public ICollection<ProgramDay> ProgramDays { get; set; } // Расписание по дням
    }
}
