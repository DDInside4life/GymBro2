using System;
using System.Collections.Generic;

namespace GymBro.Domain.Entities
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        // public string EquipmentNeeded { get; set; } // Удалено или закомментировано
        public string TechniqueTips { get; set; }
        public string CommonMistakes { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }

        public int DefaultSets { get; set; }
        public int DefaultRepsMin { get; set; }
        public int DefaultRepsMax { get; set; }
        public TimeSpan RestBetweenSets { get; set; }

        public int? TrainingProgramId { get; set; }
        public int? ProgramDayId { get; set; }

        public TrainingProgram TrainingProgram { get; set; }
        public ICollection<Equipment> Equipment { get; set; }
    }
}