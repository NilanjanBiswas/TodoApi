using TodoApi.DatabaseContext;

namespace TodoApi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private ITodoRepository? _todoRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public ITodoRepository Todos => _todoRepository ??= new TodoRepository(_context);

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
