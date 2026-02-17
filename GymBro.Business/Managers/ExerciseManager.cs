using GymBro.Domain.Entities;
using GymBro.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GymBro.Business.Managers
{
    public class ExerciseManager : BaseManager
    {
        private readonly IRepository<Exercise> _exerciseRepository;

        public ExerciseManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _exerciseRepository = unitOfWork.ExerciseRepository; // нужно добавить в IUnitOfWork
        }

        public IEnumerable<Exercise> GetAllExercises()
        {
            return _exerciseRepository.GetAll().ToList();
        }

        public async Task<IEnumerable<Exercise>> GetAllExercisesAsync()
        {
            return await _exerciseRepository.FindAsync(e => true);
        }

        public Exercise GetExerciseById(int id)
        {
            return _exerciseRepository.Get(id, "Equipment");
        }

        public IEnumerable<Exercise> GetExercisesByProgram(int programId)
        {
            return _exerciseRepository.Find(e => e.TrainingProgramId == programId).ToList();
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByProgramAsync(int programId)
        {
            return await _exerciseRepository.FindAsync(e => e.TrainingProgramId == programId);
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByEquipmentIdAsync(int equipmentId)
        {
            return await _exerciseRepository.FindAsync(e => e.Equipment.Any(eq => eq.Id == equipmentId));
        }

        public async Task CreateExerciseAsync(Exercise exercise)
        {
            _exerciseRepository.Create(exercise);
            await Task.Run(() => _unitOfWork.SaveChanges());
        }

    }
}