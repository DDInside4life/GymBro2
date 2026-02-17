using GymBro.Domain.Entities;
using GymBro.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymBro.Business.Managers
{
    public class EquipmentManager : BaseManager
    {
        private readonly IRepository<Equipment> _equipmentRepository;
        private readonly IRepository<Exercise> _exerciseRepository;

        public EquipmentManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _equipmentRepository = unitOfWork.EquipmentRepository;
            _exerciseRepository = unitOfWork.ExerciseRepository;
        }

        public async Task<IEnumerable<Equipment>> GetAllEquipmentAsync()
        {
            return await _equipmentRepository.FindAsync(e => true);
        }

        public async Task<Equipment> GetEquipmentByIdAsync(int id)
        {
            return await Task.Run(() => _equipmentRepository.Get(id, "Exercises"));
        }

        public async Task CreateEquipmentAsync(Equipment equipment)
        {
            _equipmentRepository.Create(equipment);
            await Task.Run(() => _unitOfWork.SaveChanges());
        }

        public async Task UpdateEquipmentAsync(Equipment equipment)
        {
            _equipmentRepository.Update(equipment);
            await Task.Run(() => _unitOfWork.SaveChanges());
        }

        public async Task<bool> DeleteEquipmentAsync(int id)
        {
            var result = _equipmentRepository.Delete(id);
            if (result) await Task.Run(() => _unitOfWork.SaveChanges());
            return result;
        }

        public async Task<IEnumerable<Exercise>> GetExercisesForEquipmentAsync(int equipmentId)
        {
            return await _exerciseRepository.FindAsync(e => e.Equipment.Any(eq => eq.Id == equipmentId));
        }

        public async Task AddExerciseToEquipmentAsync(int equipmentId, int exerciseId)
        {
            var equipment = await GetEquipmentByIdAsync(equipmentId);
            var exercise = await _exerciseRepository.FindAsync(e => e.Id == exerciseId);
            if (equipment != null && exercise != null)
            {
                var exerciseEntity = exercise.FirstOrDefault();
                if (!equipment.Exercises.Contains(exerciseEntity))
                {
                    equipment.Exercises.Add(exerciseEntity);
                    await UpdateEquipmentAsync(equipment);
                }
            }
        }

        public async Task RemoveExerciseFromEquipmentAsync(int equipmentId, int exerciseId)
        {
            var equipment = await GetEquipmentByIdAsync(equipmentId);
            var exercise = equipment?.Exercises?.FirstOrDefault(e => e.Id == exerciseId);
            if (equipment != null && exercise != null)
            {
                equipment.Exercises.Remove(exercise);
                await UpdateEquipmentAsync(equipment);
            }
        }
    }
}