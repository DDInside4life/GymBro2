using System.Collections.Generic;

namespace GymBro.Domain.Entities
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }          // Название оборудования (штанга, гантели, скамья...)
        public string Description { get; set; }    // Описание (опционально)

        // Навигационное свойство для связи многие-ко-многим
        public ICollection<Exercise> Exercises { get; set; }

        public Equipment()
        {
            Exercises = [];
        }
    }
}