using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Models.Todo.Requests;
using TodoApp.Models.Todo.Responses;
using TodoApp.Utilities;

namespace TodoApp.Client.HttpRepository
{
    public interface ITodoHttpRepository
    {
        Task<Result<List<ListTodosResponseModel>>> GetTodos();
        Task<Result<ReadTodoResponseModel>> GetOneTodo(Guid id);
        Task<Result> CreateTodo(CreateTodoRequestModel model);
        Task<Result> UpdateTodo(UpdateTodoRequestModel model);
        Task<Result> DeleteTodo(Guid id);
    }
}
