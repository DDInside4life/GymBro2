using GymBro.Business.Managers;
using GymBro.DAL.Data;
using GymBro.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GymBro.Business.Infrastructure
{
    /// <summary>
    /// Фабрика для создания менеджеров
    /// Паттерн Factory - используется для внедрения зависимостей
    /// </summary>
    public class ManagersFactory
    {
        private readonly IUnitOfWork _unitOfWork;
        private UserProfileManager _userProfileManager;
        private TrainingProgramManager _trainingProgramManager;
        private UserManager _userManager;
        private ExerciseManager _exerciseManager;
        private EquipmentManager _equipmentManager;

        public EquipmentManager GetEquipmentManager()
        {
            return _equipmentManager ??= new EquipmentManager(_unitOfWork);
        }


        public ExerciseManager GetExerciseManager()
        {
            return _exerciseManager ??= new ExerciseManager(_unitOfWork);
        }

        public UserManager GetUserManager()
        {
            return _userManager ??= new UserManager(_unitOfWork);
        }



        public ManagersFactory(string connectionString)
        {
            // Создаем UnitOfWork с переданной строкой подключения
            _unitOfWork = new GymBro.DAL.Repositories.EFUnitOfWork(connectionString);
        }



        /// <summary>
        /// Создать фабрику с конфигурацией из файла appsettings.json
        /// </summary>
        public ManagersFactory() : this(GetConnectionStringFromConfig())
        {
        }

        private static string GetConnectionStringFromConfig()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Получить менеджер профилей пользователей
        /// </summary>
        public UserProfileManager GetUserProfileManager()
        {
            return _userProfileManager ??= new UserProfileManager(_unitOfWork);
        }

        /// <summary>
        /// Получить менеджер тренировочных программ
        /// </summary>
        public TrainingProgramManager GetTrainingProgramManager()
        {
            return _trainingProgramManager ??= new TrainingProgramManager(_unitOfWork);
        }

        public void InitializeDatabase()
        {
            // Получаем доступ к контексту через рефлексию или добавим в EFUnitOfWork свойство для контекста.
            using (var context = new GymBro.DAL.Data.GymBroContext(
                new DbContextOptionsBuilder<GymBroContext>()
                    .UseSqlServer(GetConnectionStringFromConfig())
                    .Options))
            {
                DbInitializer.Initialize(context);
            }
        }

  
    }
}