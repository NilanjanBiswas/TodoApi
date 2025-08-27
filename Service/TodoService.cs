using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoApi.Dtos;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Service
{
    public class TodoService : ITodoService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public TodoService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<PagedResult<TodoDto>> GetTodosAsync(Guid userId, int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _uow.Todos.GetByUser(userId);
            var total = await query.LongCountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();

            var dtos = items.Select(i => _mapper.Map<TodoDto>(i)).ToArray();

            return new PagedResult<TodoDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize),
                Items = dtos
            };
        }

        public async Task<TodoDto?> GetByIdAsync(Guid userId, Guid id)
        {
            var item = await _uow.Todos.GetByIdAsync(id);
            if (item == null || item.UserId != userId) return null;
            return _mapper.Map<TodoDto>(item);
        }

        public async Task<TodoDto> CreateAsync(Guid userId, CreateTodoDto dto)
        {
            var entity = _mapper.Map<TodoItem>(dto);
            entity.UserId = userId;

            await _uow.Todos.CreateAsync(entity);
            await _uow.SaveAsync();

            return _mapper.Map<TodoDto>(entity);
        }

        public async Task<bool> UpdateAsync(Guid userId, Guid id, UpdateTodoDto dto)
        {
            var entity = await _uow.Todos.GetByIdAsync(id);
            if (entity == null || entity.UserId != userId) return false;

            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.IsCompleted = dto.IsCompleted;
            entity.DueDate = dto.DueDate;
            _uow.Todos.Update(entity);

            return await _uow.SaveAsync();
        }

        public async Task<bool> DeleteAsync(Guid userId, Guid id)
        {
            var entity = await _uow.Todos.GetByIdAsync(id);
            if (entity == null || entity.UserId != userId) return false;

            _uow.Todos.Delete(entity);
            return await _uow.SaveAsync();
        }
    }
}
