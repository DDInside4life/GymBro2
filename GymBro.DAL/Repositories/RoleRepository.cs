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
    public class RoleRepository : IRepository<Role>
    {
        private readonly GymBroContext _context;
        private readonly DbSet<Role> _dbSet;

        public RoleRepository(GymBroContext context)
        {
            _context = context;
            _dbSet = context.Roles;
        }

        public IQueryable<Role> GetAll() => _dbSet.AsQueryable();
        public Role Get(int id, params string[] includes)
        {
            IQueryable<Role> query = _dbSet;
            foreach (var include in includes)
                query = query.Include(include);
            return query.FirstOrDefault(r => r.Id == id);
        }
        public IQueryable<Role> Find(Expression<Func<Role, bool>> predicate) => _dbSet.Where(predicate);
        public async Task<IEnumerable<Role>> FindAsync(Expression<Func<Role, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();
        public void Create(Role entity) => _dbSet.Add(entity);
        public void Update(Role entity) => _context.Entry(entity).State = EntityState.Modified;
        public bool Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null) return false;
            _dbSet.Remove(entity);
            return true;
        }
    }
}