using System.Linq.Expressions;

namespace Library.API.Services
{
    public interface IRepositoryBase<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> SaveAsync();
    }

    public interface IRepositoryBase2<T, TId>
    {
        Task<T?> GetByIdAsync(TId id);
        Task<bool> IsExistAsync(TId id);
    }
}
