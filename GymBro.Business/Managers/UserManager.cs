using BCrypt.Net;
using GymBro.Domain.Entities;
using GymBro.Domain.Interfaces;
using System.Linq;
using System.Threading.Tasks;

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
            // Сначала ищем пользователя по логину
            var user = _userRepository.Find(u => u.Login == login).FirstOrDefault();
            if (user == null)
                return null;

            // Явно загружаем пользователя с ролями (через Get с Include)
            user = _userRepository.Get(user.Id, "Roles");

            // Проверяем пароль
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

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _unitOfWork.RolesRepository.FindAsync(r => true);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _unitOfWork.UsersRepository.FindAsync(u => u.Id == id).ContinueWith(t => t.Result.FirstOrDefault());
        }

        public async Task CreateUserAsync(User user, string password)
        {
            if (_userRepository.Find(u => u.Login == user.Login).Any())
                throw new InvalidOperationException("Логин уже занят");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            user.Language = "ru";
            _userRepository.Create(user);
            await _unitOfWork.SaveChangesAsync(); // если есть асинхронный SaveChanges, иначе синхронный
        }

        public async Task UpdateUserAsync(User user, string newPassword = null)
        {
            var existing = await _userRepository.FindAsync(u => u.Id == user.Id).ContinueWith(t => t.Result.FirstOrDefault());
            if (existing == null) return;

            existing.Login = user.Login;
            existing.FullName = user.FullName;
            existing.Email = user.Email;
            existing.Roles = user.Roles;
            if (!string.IsNullOrWhiteSpace(newPassword))
                existing.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            _userRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync();
        }



        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = _userRepository.Get(id, "UserProfile", "Roles");
            if (user == null)
                return false;

            if (user.UserProfileId.HasValue)
            {
                _unitOfWork.UserProfilesRepository.Delete(user.UserProfileId.Value);
            }

            var deleted = _userRepository.Delete(id);
            if (!deleted)
                return false;

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}