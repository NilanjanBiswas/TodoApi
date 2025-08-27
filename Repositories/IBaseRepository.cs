using System.Linq.Expressions;

namespace TodoApi.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        IQueryable<T> FindByConditionWithTracking(Expression<Func<T, bool>> expression);

        void Create(T entity);
        Task CreateAsync(T entity);
        void CreateRange(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);

        Task<int> SaveChangesAsync();
    }
}
