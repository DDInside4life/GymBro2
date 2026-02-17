using GymBro.Domain.Entities;
using GymBro.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;


namespace GymBro.Business.Managers
{
    /// <summary>
    /// Менеджер для работы с профилями пользователей
    /// Содержит бизнес-логику для UserProfile
    /// </summary>
    public class UserProfileManager : BaseManager
    {
        public UserProfileManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Получить все профили пользователей
        /// </summary>
        public IEnumerable<UserProfile> GetAllUserProfiles()
        {
            return _userProfileRepository.GetAll().ToList();
        }

        /// <summary>
        /// Получить профиль по ID
        /// </summary>
        public UserProfile GetUserProfileById(int id)
        {
            return _userProfileRepository.Get(id, "TrainingPrograms");
        }

        /// <summary>
        /// Создать новый профиль пользователя
        /// </summary>
        public void CreateUserProfile(UserProfile userProfile)
        {
            _userProfileRepository.Create(userProfile);
            _unitOfWork.SaveChanges();
        }

        /// <summary>
        /// Обновить профиль пользователя
        /// </summary>
        public void UpdateUserProfile(UserProfile userProfile)
        {
            _userProfileRepository.Update(userProfile);
            _unitOfWork.SaveChanges();
        }

        /// <summary>
        /// Удалить профиль пользователя
        /// </summary>
        public bool DeleteUserProfile(int id)
        {
            var result = _userProfileRepository.Delete(id);
            if (result)
            {
                _unitOfWork.SaveChanges();
            }
            return result;
        }

        /// <summary>
        /// Рассчитать ИМТ (Индекс массы тела)
        /// </summary>
        public decimal CalculateBMI(UserProfile userProfile)
        {
            if (userProfile.Height == 0)
                return 0;

            // Формула: вес (кг) / (рост (м) * рост (м))
            var heightInMeters = userProfile.Height / 100;
            return userProfile.Weight / (heightInMeters * heightInMeters);
        }

        /// <summary>
        /// Получить рекомендации на основе профиля
        /// </summary>
        public string GetRecommendations(UserProfile userProfile)
        {
            var bmi = CalculateBMI(userProfile);

            if (bmi < 18.5m)
            {
                return "Рекомендуется программа для набора массы";
            }
            else if (bmi > 25)
            {
                return "Рекомендуется программа для похудения";
            }
            else
            {
                return "Рекомендуется программа для поддержания формы";
            }
        }
    }
}