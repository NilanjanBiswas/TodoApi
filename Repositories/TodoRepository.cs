using Microsoft.EntityFrameworkCore;
using TodoApi.DatabaseContext;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class TodoRepository : RepositoryBase<TodoItem>, ITodoRepository
    {
        public TodoRepository(ApplicationDbContext db) : base(db) { }

        public IQueryable<TodoItem> GetByUser(Guid userId)
        {
            return RepositoryContext.Todos
                   .Where(t => t.UserId == userId)
                   .AsNoTracking()
                   .OrderByDescending(t => t.CreatedAt);
        }

        public async Task<TodoItem?> GetByIdAsync(Guid id)
        {
            return await RepositoryContext.Todos.FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
