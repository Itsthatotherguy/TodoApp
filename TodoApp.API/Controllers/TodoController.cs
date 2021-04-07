using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TodoApp.Models.Todo.Requests;
using TodoApp.Services.Exceptions;
using TodoApp.Services.TodoServices;

namespace TodoApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _todoService.ListTodos();

                return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
            }
            catch (System.Exception)
            {
                return UnprocessableEntity("Error fetching DB entities");
            }
        }

        [HttpGet("{id}", Name = "GetOne")]
        public async Task<IActionResult> GetOne(Guid id)
        {
            try
            {
                var result = await _todoService.ReadTodo(id);

                if (result.IsSuccess)
                {
                    return Ok(result.Value);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            catch (DbTransactionException)
            {
                return UnprocessableEntity("Error reading entity from DB");                
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTodoRequestModel model)
        {
            try
            {
                var result = await _todoService.CreateTodo(model);

                return result.IsSuccess 
                    ? CreatedAtRoute("GetOne", new { Id = result.Value.Id }, result.Value) 
                    : BadRequest(result.Errors);
            }
            catch (DbTransactionException)
            {
                return UnprocessableEntity("Error saving DB transaction");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTodoRequestModel model)
        {
            if (id == Guid.Empty || id != model.Id)
            {
                return BadRequest("Id mismatch");
            }

            try
            {
                var result = await _todoService.UpdateTodo(model);

                if (result.IsSuccess)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            catch (DbTransactionException)
            {
                return UnprocessableEntity("Error updating entity on DB");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _todoService.DeleteTodo(id);

                if (result.IsSuccess)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            catch (DbTransactionException)
            {
                return UnprocessableEntity("Error deleting entity from DB");
            }
        }
    }
}
