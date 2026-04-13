using Microsoft.EntityFrameworkCore;
using REPOSITORY.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace REPOSITORY.Repository
{
    internal class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataContext _context;
        private readonly DbSet<T> _entities;
        public GenericRepository(DataContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public async Task Add(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await _entities.AddAsync(entity);
        }
        public void Update(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _entities.Update(entity);
        }

        public void Delete(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _entities.Remove(entity);
        }

        public void DeleteMultiple(IEnumerable<T> entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _entities.RemoveRange(entity);
        }
        public async Task<T?> GetById(int id) => await _entities.FindAsync(id);
        public List<T> GetByExp(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>()
                .Where(predicate)
                .AsNoTracking()
                .ToList();
        }

        public async Task<IEnumerable<T>> GetAll() => await _entities.ToListAsync();

        public async Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> expression)
        {
            var entities = _entities.Where(expression);

            var result = await entities.ToListAsync();



            return result;
        }

        public Task<T?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
