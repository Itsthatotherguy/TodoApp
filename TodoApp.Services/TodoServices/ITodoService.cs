using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Models.Todo;
using TodoApp.Utilities;

namespace TodoApp.Services.TodoServices
{
    public interface ITodoService
    {
        Task<Result<GetOneTodoDto>> CreateTodo(CreateTodoDto model);
        Task<Result> UpdateTodo(UpdateTodoDto dto);
        Task<Result> PartialUpdate(Guid id, JsonPatchDocument<PartialUpdateTodoDto> patchDocument);
        Task<Result> DeleteTodo(Guid id);
        Task<Result<IEnumerable<GetAllTodosDto>>> ListTodos();
        Task<Result<GetOneTodoDto>> GetOneTodo(Guid id);
    }
}
