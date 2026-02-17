using GymBro.DAL.Data;
using GymBro.Domain.Entities;
using GymBro.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GymBro.DAL.Repositories
{
    /// <summary>
    /// Репозиторий для работы с тренировочными программами
    /// Реализация IRepository с использованием Entity Framework
    /// </summary>
    public class TrainingProgramRepository : IRepository<TrainingProgram>
    {
        private readonly GymBroContext _context;
        private readonly DbSet<TrainingProgram> _dbSet;

        public TrainingProgramRepository(GymBroContext context)
        {
            _context = context;
            _dbSet = context.TrainingPrograms;
        }

        public IQueryable<TrainingProgram> GetAll() => _dbSet.Include(p => p.UserProfile).AsQueryable();

        public TrainingProgram Get(int id, params string[] includes)
        {
            IQueryable<TrainingProgram> query = _dbSet;

            // Включаем связанные свойства
            query = query.Include(p => p.UserProfile);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<TrainingProgram> Find(Expression<Func<TrainingProgram, bool>> predicate)
        {
            return _dbSet.Include(p => p.UserProfile).Where(predicate);
        }

        public async Task<IEnumerable<TrainingProgram>> FindAsync(Expression<Func<TrainingProgram, bool>> predicate)
        {
            return await _dbSet.Include(p => p.Exercises).ThenInclude(e => e.Equipment).Where(predicate).ToListAsync();
        }

        public void Create(TrainingProgram entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(TrainingProgram entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public bool Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            return true;
        }


    }
}