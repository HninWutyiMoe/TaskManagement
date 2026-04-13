using System.Linq.Expressions;

namespace REPOSITORY.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetById(int id);
        Task<T?> GetByIdAsync(int id);
        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteMultiple(IEnumerable<T> entity);
        Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> expression);
        List<T> GetByExp(Expression<Func<T, bool>> predicate);
    }
}
