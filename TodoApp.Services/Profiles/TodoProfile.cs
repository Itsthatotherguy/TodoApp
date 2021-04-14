using AutoMapper;
using System;
using TodoApp.Domain.Entities;
using TodoApp.Models.Todo;

namespace TodoApp.Services.Profiles
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            // create todo
            CreateMap<CreateTodoDto, Todo>();

            // list todos
            CreateMap<Todo, GetAllTodosDto>();

            // get one todo
            CreateMap<Todo, GetOneTodoDto>();

            // update todo
            CreateMap<UpdateTodoDto, Todo>()
                .ForMember(dest => dest.DueDate, opt => opt.Condition(src => src.DueDate > DateTime.MinValue));

            CreateMap<GetOneTodoDto, UpdateTodoDto>();

            // partial update todo
            CreateMap<GetOneTodoDto, PartialUpdateTodoDto>();
            CreateMap<PartialUpdateTodoDto, UpdateTodoDto>();
        }
    }
}
