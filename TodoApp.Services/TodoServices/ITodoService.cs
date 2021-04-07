using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Models.Todo.Requests;
using TodoApp.Models.Todo.Responses;
using TodoApp.Utilities;

namespace TodoApp.Services.TodoServices
{
    public interface ITodoService
    {
        Task<Result<CreateTodoResponseModel>> CreateTodo(CreateTodoRequestModel model);
        Task<Result> UpdateTodo(UpdateTodoRequestModel model);
        Task<Result> DeleteTodo(Guid id);
        Task<Result<IEnumerable<ListTodosResponseModel>>> ListTodos();
        Task<Result<ReadTodoResponseModel>> ReadTodo(Guid id);
    }
}
