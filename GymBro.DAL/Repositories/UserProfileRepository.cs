using GymBro.DAL.Data;
using GymBro.Domain;

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
    /// Репозиторий для работы с профилями пользователей
    /// Реализация IRepository с использованием Entity Framework
    /// </summary>
    public class UserProfileRepository : IRepository<UserProfile>
    {
        private readonly GymBroContext _context;
        private readonly DbSet<UserProfile> _dbSet;

        public UserProfileRepository(GymBroContext context)
        {
            _context = context;
            _dbSet = context.UserProfiles;
        }

        public IQueryable<UserProfile> GetAll() => _dbSet.AsQueryable();

        public UserProfile Get(int id, params string[] includes)
        {
            IQueryable<UserProfile> query = _dbSet;

            // Включаем связанные свойства (eager loading)
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.FirstOrDefault(u => u.Id == id);
        }

        public IQueryable<UserProfile> Find(Expression<Func<UserProfile, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public async Task<IEnumerable<UserProfile>> FindAsync(Expression<Func<UserProfile, bool>> predicate)
        {
            // Асинхронный метод поиска
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public void Create(UserProfile entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(UserProfile entity)
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