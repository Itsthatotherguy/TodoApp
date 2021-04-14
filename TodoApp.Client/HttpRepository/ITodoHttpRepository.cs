using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Models.Todo;
using TodoApp.Utilities;

namespace TodoApp.Client.HttpRepository
{
    public interface ITodoHttpRepository
    {
        Task<Result<List<GetAllTodosDto>>> GetTodos();
        Task<Result<GetOneTodoDto>> GetOneTodo(Guid id);
        Task<Result> CreateTodo(CreateTodoDto model);
        Task<Result> UpdateTodo(UpdateTodoDto model);
        Task<Result> MarkTodoComplete(Guid id);
        Task<Result> MarkTodoIncomplete(Guid id);
        Task<Result> DeleteTodo(Guid id);
    }
}
