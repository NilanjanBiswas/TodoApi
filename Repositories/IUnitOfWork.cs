namespace TodoApi.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ITodoRepository Todos { get; }
        Task<bool> SaveAsync();
    }
}
