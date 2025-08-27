using TodoApi.Dtos;

namespace TodoApi.Service
{
    public interface ITodoService
    {
        Task<PagedResult<TodoDto>> GetTodosAsync(Guid userId, int page, int pageSize);
        Task<TodoDto?> GetByIdAsync(Guid userId, Guid id);
        Task<TodoDto> CreateAsync(Guid userId, CreateTodoDto dto);
        Task<bool> UpdateAsync(Guid userId, Guid id, UpdateTodoDto dto);
        Task<bool> DeleteAsync(Guid userId, Guid id);
    }
}
