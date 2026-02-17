using BCrypt.Net;
using GymBro.Domain.Entities;
using GymBro.Domain.Interfaces;
using System.Linq;

namespace GymBro.Business.Managers
{
    public class UserManager : BaseManager
    {
        private readonly IRepository<User> _userRepository;

        public UserManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _userRepository = unitOfWork.UsersRepository;
        }

        public User ValidateUser(string login, string password)
        {
            var user = _userRepository.Find(u => u.Login == login).FirstOrDefault();
            if (user == null)
                return null;

            // Используем полное имя для гарантии
            bool valid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return valid ? user : null;
        }

        public User Register(string login, string password, string email, string fullName)
        {
            if (_userRepository.Find(u => u.Login == login).Any())
                throw new InvalidOperationException("Логин уже занят");

            var user = new User
            {
                Login = login,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Email = email,
                FullName = fullName,
                Language = "ru", // по умолчанию
                Roles = new List<Role>()
            };

            // Добавляем роль User
            var userRole = _unitOfWork.RolesRepository.Find(r => r.Name == "User").FirstOrDefault();
            if (userRole != null)
                user.Roles.Add(userRole);

            _userRepository.Create(user);
            _unitOfWork.SaveChanges();
            return user;
        }

        public bool ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var user = _userRepository.Get(userId);
            if (user == null)
                return false;

            if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash))
                return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _userRepository.Update(user);
            _unitOfWork.SaveChanges();
            return true;
        }

        public void UpdateUserLanguage(int userId, string language)
        {
            var user = _userRepository.Get(userId);
            if (user != null)
            {
                user.Language = language;
                _userRepository.Update(user);
                _unitOfWork.SaveChanges();
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.FindAsync(u => true);
        }
    }
}