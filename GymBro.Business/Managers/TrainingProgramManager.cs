using GymBro.Domain.Entities;
using GymBro.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GymBro.Business.Managers
{
    /// <summary>
    /// Менеджер для работы с тренировочными программами
    /// Содержит бизнес-логику для TrainingProgram
    /// </summary>
    public class TrainingProgramManager : BaseManager
    {
        public TrainingProgramManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Получить все тренировочные программы
        /// </summary>
        public IEnumerable<TrainingProgram> GetAllTrainingPrograms()
        {
            return _trainingProgramRepository.GetAll().ToList();
        }

        /// <summary>
        /// Получить программу по ID
        /// </summary>
        public TrainingProgram GetTrainingProgramById(int id)
        {
            return _trainingProgramRepository.Get(id, "UserProfile");
        }

        /// <summary>
        /// Получить программы определенного пользователя (синхронно)
        /// </summary>
        public IEnumerable<TrainingProgram> GetProgramsByUser(int userId)
        {
            return _trainingProgramRepository.Find(p => p.UserProfileId == userId).ToList();
        }

        /// <summary>
        /// Получить программы определенного пользователя (асинхронно)
        /// ТРЕБОВАНИЕ ЛАБОРАТОРНОЙ: один из методов сделать асинхронным
        /// </summary>
        public async Task<IEnumerable<TrainingProgram>> GetProgramsByUserAsync(int userId)
        {
            return await _trainingProgramRepository.FindAsync(p => p.UserProfileId == userId);
        }

        /// <summary>
        /// Найти программы по условию
        /// </summary>
        public IEnumerable<TrainingProgram> FindPrograms(Expression<Func<TrainingProgram, bool>> predicate)
        {
            return _trainingProgramRepository.Find(predicate).ToList();
        }

        /// <summary>
        /// Создать новую программу
        /// </summary>
        public void CreateTrainingProgram(TrainingProgram program)
        {
            program.CreatedDate = DateTime.Now;
            var selectedExerciseIds = program.Exercises?
               .Where(e => e != null && e.Id > 0)
               .Select(e => e.Id)
               .Distinct()
               .ToList() ?? new List<int>();

            // Привязку упражнений делаем отдельно, чтобы избежать проблем с отслеживанием
            // сущностей из разных DbContext.
            program.Exercises = null;
            _trainingProgramRepository.Create(program);
            _unitOfWork.SaveChanges();
            if (selectedExerciseIds.Count == 0)
            {
                return;
            }

            var exercisesToAttach = _unitOfWork.ExerciseRepository
                .Find(e => selectedExerciseIds.Contains(e.Id))
                .ToList();

            foreach (var exercise in exercisesToAttach)
            {
                exercise.TrainingProgramId = program.Id;
                _unitOfWork.ExerciseRepository.Update(exercise);
            }

            _unitOfWork.SaveChanges();
        }

        /// <summary>
        /// Обновить программу
        /// </summary>
        public void UpdateTrainingProgram(TrainingProgram program)
        {
            var existingProgram = _trainingProgramRepository.Get(program.Id, "Exercises");
            if (existingProgram == null)
            {
                return;
            }

            existingProgram.Name = program.Name;
            existingProgram.Description = program.Description;
            existingProgram.ProgramType = program.ProgramType;
            existingProgram.DurationWeeks = program.DurationWeeks;
            existingProgram.Difficulty = program.Difficulty;
            existingProgram.WorkoutsPerWeek = program.WorkoutsPerWeek;

            var selectedExerciseIds = program.Exercises?
                .Where(e => e != null && e.Id > 0)
                .Select(e => e.Id)
                .Distinct()
                .ToHashSet() ?? new HashSet<int>();

            var currentExerciseIds = existingProgram.Exercises?
                .Select(e => e.Id)
                .ToHashSet() ?? new HashSet<int>();

            var exerciseIdsToRemove = currentExerciseIds.Except(selectedExerciseIds).ToList();
            if (exerciseIdsToRemove.Count > 0)
            {
                var exercisesToRemove = _unitOfWork.ExerciseRepository
                    .Find(e => exerciseIdsToRemove.Contains(e.Id))
                    .ToList();

                foreach (var exercise in exercisesToRemove)
                {
                    exercise.TrainingProgramId = null;
                    _unitOfWork.ExerciseRepository.Update(exercise);
                }
            }

            var exerciseIdsToAdd = selectedExerciseIds.Except(currentExerciseIds).ToList();
            if (exerciseIdsToAdd.Count > 0)
            {
                var exercisesToAdd = _unitOfWork.ExerciseRepository
                    .Find(e => exerciseIdsToAdd.Contains(e.Id))
                    .ToList();

                foreach (var exercise in exercisesToAdd)
                {
                    exercise.TrainingProgramId = existingProgram.Id;
                    _unitOfWork.ExerciseRepository.Update(exercise);
                }
            }

            _trainingProgramRepository.Update(existingProgram);
            _unitOfWork.SaveChanges();
        }

        /// <summary>
        /// Удалить программу
        /// </summary>
        public bool DeleteTrainingProgram(int id)
        {
            var result = _trainingProgramRepository.Delete(id);
            if (result)
            {
                _unitOfWork.SaveChanges();
            }
            return result;
        }

        public async Task<IEnumerable<TrainingProgram>> GetAllTrainingProgramsAsync()
        {
            return await _trainingProgramRepository.FindAsync(p => true);
        }

        public async Task<IEnumerable<TrainingProgram>> GetAllTemplatesAsync()
        {
            return await _trainingProgramRepository.FindAsync(p => p.IsTemplate);
        }

        /// <summary>
        /// Создать программу на основе профиля пользователя
        /// </summary>
        public TrainingProgram GenerateProgramForUser(UserProfile userProfile)
        {
            var program = new TrainingProgram
            {
                UserProfileId = userProfile.Id,
                CreatedDate = DateTime.Now
            };

            // Генерация программы на основе данных пользователя
            if (userProfile.TrainingGoal == "Похудение")
            {
                program.Name = "Программа для похудения";
                program.Description = "Кардио и силовые тренировки для сжигания жира";
                program.ProgramType = "Кардио";
                program.DurationWeeks = 8;
                program.Difficulty = 3;
                //program.WorkoutsPerWeek = 4;
            }
            else if (userProfile.TrainingGoal == "Набор массы")
            {
                program.Name = "Программа для набора массы";
                program.Description = "Силовые тренировки для роста мышц";
                program.ProgramType = "Силовая";
                program.DurationWeeks = 12;
                program.Difficulty = 4;
                //program.WorkoutsPerWeek = 3;
            }
            else
            {
                program.Name = "Программа для поддержания формы";
                program.Description = "Сбалансированные тренировки";
                program.ProgramType = "Фулбоди";
                program.DurationWeeks = 6;
                program.Difficulty = 2;
                //program.WorkoutsPerWeek = 3;
            }

            return program;
        }

        public async Task<IEnumerable<TrainingProgram>> GetProgramsForCurrentUserAsync(int? userProfileId, bool isAdmin)
        {
            if (isAdmin)
            {
                // Админ видит все программы (включая шаблоны)
                return await _trainingProgramRepository.FindAsync(p => true);
            }
            else
            {
                // Обычный пользователь видит свои программы и шаблонные
                return await _trainingProgramRepository.FindAsync(p => p.UserProfileId == userProfileId || p.IsTemplate);
            }
        }

        public async Task<TrainingProgram> GetTrainingProgramByIdAsync(int id)
        {
            return await Task.Run(() => _trainingProgramRepository.Get(id, "Exercises.Equipment"));
        }
    }
}