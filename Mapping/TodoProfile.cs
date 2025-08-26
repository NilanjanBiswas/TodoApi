using AutoMapper;
using TodoApi.Dtos;
using TodoApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TodoApi.Mapping
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<TodoItem, TodoDto>();
            CreateMap<CreateTodoDto, TodoItem>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.IsCompleted, opt => opt.MapFrom(_ => false));


            CreateMap<UpdateTodoDto, TodoItem>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.UserId, opt => opt.Ignore());
        }
    }
}
