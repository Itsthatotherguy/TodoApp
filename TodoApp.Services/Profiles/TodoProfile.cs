using AutoMapper;
using TodoApp.Domain.Entities;
using TodoApp.Models.Todo.Requests;
using TodoApp.Models.Todo.Responses;

namespace TodoApp.Services.Profiles
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            // create todo
            CreateMap<CreateTodoRequestModel, Todo>();
            CreateMap<Todo, CreateTodoResponseModel>();

            // list todos
            CreateMap<Todo, ListTodosResponseModel>();

            // read todo
            CreateMap<Todo, ReadTodoResponseModel>();

            // update todo
            CreateMap<UpdateTodoRequestModel, Todo>();
        }
    }
}
