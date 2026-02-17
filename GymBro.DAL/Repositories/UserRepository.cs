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
    public class UserRepository : IRepository<User>
    {
        private readonly GymBroContext _context;
        private readonly DbSet<User> _dbSet;

        public UserRepository(GymBroContext context)
        {
            _context = context;
            _dbSet = context.Users;
        }

        public IQueryable<User> GetAll() => _dbSet.AsQueryable();
        public User Get(int id, params string[] includes)
        {
            IQueryable<User> query = _dbSet;
            foreach (var include in includes)
                query = query.Include(include);
            return query.FirstOrDefault(u => u.Id == id);
        }
        public IQueryable<User> Find(Expression<Func<User, bool>> predicate) => _dbSet.Where(predicate);
        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();
        public void Create(User entity) => _dbSet.Add(entity);
        public void Update(User entity) => _context.Entry(entity).State = EntityState.Modified;
        public bool Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null) return false;
            _dbSet.Remove(entity);
            return true;
        }
    }
}