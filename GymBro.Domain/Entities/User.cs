using System.Collections.Generic;

namespace GymBro.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Language { get; set; } = "ru"; // язык по умолчанию

        public int? UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }

        // Роли пользователя
        public ICollection<Role> Roles { get; set; }

        public User()
        {
            Roles = new List<Role>();
        }
    }
}