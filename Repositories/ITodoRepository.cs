using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface ITodoRepository : IBaseRepository<TodoItem>
    {
        IQueryable<TodoItem> GetByUser(Guid userId);
        Task<TodoItem?> GetByIdAsync(Guid id);
    }
}
