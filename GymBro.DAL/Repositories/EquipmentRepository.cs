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
    public class EquipmentRepository : IRepository<Equipment>
    {
        private readonly GymBroContext _context;
        private readonly DbSet<Equipment> _dbSet;

        public EquipmentRepository(GymBroContext context)
        {
            _context = context;
            _dbSet = context.Equipment;
        }

        public IQueryable<Equipment> GetAll() => _dbSet.Include(e => e.Exercises).AsQueryable();

        public Equipment Get(int id, params string[] includes)
        {
            IQueryable<Equipment> query = _dbSet;
            foreach (var include in includes)
                query = query.Include(include);
            return query.FirstOrDefault(e => e.Id == id);
        }

        public IQueryable<Equipment> Find(Expression<Func<Equipment, bool>> predicate)
        {
            return _dbSet.Include(e => e.Exercises).Where(predicate);
        }

        public async Task<IEnumerable<Equipment>> FindAsync(Expression<Func<Equipment, bool>> predicate)
        {
            return await _dbSet.Include(e => e.Exercises).Where(predicate).ToListAsync();
        }

        public void Create(Equipment entity) => _dbSet.Add(entity);

        public void Update(Equipment entity) => _context.Entry(entity).State = EntityState.Modified;

        public bool Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null) return false;
            _dbSet.Remove(entity);
            return true;
        }
    }
}