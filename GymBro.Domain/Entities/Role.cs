using System.Collections.Generic;

namespace GymBro.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Навигационное свойство для связи многие-ко-многим с User
        public ICollection<User> Users { get; set; }

        public Role()
        {
            Users = new List<User>();
        }
    }
}