using GymBro.Domain.Interfaces;

namespace GymBro.Business.Managers
{
    /// <summary>
    /// Базовый класс для менеджеров
    /// Содержит общую логику работы с репозиториями
    /// </summary>
    public abstract class BaseManager
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IRepository<Domain.Entities.TrainingProgram> _trainingProgramRepository;
        protected readonly IRepository<Domain.Entities.UserProfile> _userProfileRepository;

        protected BaseManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _trainingProgramRepository = unitOfWork.TrainingProgramsRepository;
            _userProfileRepository = unitOfWork.UserProfilesRepository;
        }
    }
}