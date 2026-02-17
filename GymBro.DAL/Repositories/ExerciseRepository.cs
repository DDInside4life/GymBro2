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
    public class ExerciseRepository : IRepository<Exercise>
    {
        private readonly GymBroContext _context;
        private readonly DbSet<Exercise> _dbSet;

        public ExerciseRepository(GymBroContext context)
        {
            _context = context;
            _dbSet = context.Exercises;
        }

        public IQueryable<Exercise> GetAll() => _dbSet.Include(e => e.Equipment).AsQueryable();

        public Exercise Get(int id, params string[] includes)
        {
            IQueryable<Exercise> query = _dbSet;
            foreach (var include in includes)
                query = query.Include(include);
            return query.FirstOrDefault(e => e.Id == id);
        }

        public IQueryable<Exercise> Find(Expression<Func<Exercise, bool>> predicate)
        {
            return _dbSet.Include(e => e.Equipment).Where(predicate);
        }

        public async Task<IEnumerable<Exercise>> FindAsync(Expression<Func<Exercise, bool>> predicate)
        {
            return await _dbSet.Include(e => e.Equipment).Where(predicate).ToListAsync();
        }

        public void Create(Exercise entity) => _dbSet.Add(entity);
        public void Update(Exercise entity) => _context.Entry(entity).State = EntityState.Modified;
        public bool Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null) return false;
            _dbSet.Remove(entity);
            return true;
        }
    }
}