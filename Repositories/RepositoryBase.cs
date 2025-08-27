using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TodoApi.DatabaseContext;

namespace TodoApi.Repositories
{
    public class RepositoryBase<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext RepositoryContext;
        public RepositoryBase(ApplicationDbContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll() => RepositoryContext.Set<T>().AsNoTracking();

        public IQueryable<T> FindByConditionWithTracking(Expression<Func<T, bool>> expression)
            => RepositoryContext.Set<T>().Where(expression);

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
            => RepositoryContext.Set<T>().Where(expression).AsNoTracking();

        public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);

        public async Task CreateAsync(T entity) => await RepositoryContext.Set<T>().AddAsync(entity);

        public void CreateRange(IEnumerable<T> entity) => RepositoryContext.Set<T>().AddRange(entity);

        public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);

        public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);

        public async Task<int> SaveChangesAsync() => await RepositoryContext.SaveChangesAsync();
    }
}
