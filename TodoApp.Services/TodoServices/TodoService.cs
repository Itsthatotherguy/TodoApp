using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Database.Contexts;
using TodoApp.Domain.Entities;
using TodoApp.Models.Todo;
using TodoApp.Services.Exceptions;
using TodoApp.Utilities;

namespace TodoApp.Services.TodoServices
{
    public class TodoService : ITodoService
    {
        private readonly ITodoDbContext _dbContext;
        private readonly IMapper _mapper;

        public TodoService(ITodoDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Result<GetOneTodoDto>> CreateTodo(CreateTodoDto model)
        {
            // check that todo with the same title doesnt exist
            if (await TitleAlreadyExists(model.Title))
            {
                return Result<GetOneTodoDto>.Failure("A todo with that title already exists");
            }

            // create todo
            var todo = _mapper.Map<Todo>(model);

            // add to db
            try
            {
                _dbContext.Todos.Add(todo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbTransactionException();
            }

            // return response
            return Result<GetOneTodoDto>.Success(
                _mapper.Map<GetOneTodoDto>(todo));
        }

        public async Task<Result> DeleteTodo(Guid id)
        {
            // check if id provided
            if (id == Guid.Empty)
            {
                return Result.Failure("No id provided");
            }

            // find todo
            var todo = await _dbContext.Todos.FirstOrDefaultAsync(todo => todo.Id == id);

            if (todo == null)
            {
                return Result.Failure("Todo not found");
            }

            // delete todo
            try
            {
                _dbContext.Todos.Remove(todo);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbTransactionException();
            }

            return Result.Success();
        }

        public async Task<Result<IEnumerable<GetAllTodosDto>>> ListTodos()
        {
            var todoList = await _dbContext.Todos.ToListAsync();

            if (todoList == null)
            {
                throw new DbTransactionException();
            }

            return Result<IEnumerable<GetAllTodosDto>>.Success(
                _mapper.Map<IEnumerable<GetAllTodosDto>>(todoList));
        }

        public async Task<Result> PartialUpdate(Guid id, JsonPatchDocument<PartialUpdateTodoDto> patchDocument)
        {
            // find existing todo
            var todoResult = await GetOneTodo(id);

            if (todoResult.IsFailure)
            {
                return Result.Failure(todoResult.Errors);
            }

            // map todo to partial update dto
            var partialUpdateDto = _mapper.Map<PartialUpdateTodoDto>(todoResult.Value);

            // apply patch doc
            patchDocument.ApplyTo(partialUpdateDto);

            // perform update
            var updateDto = _mapper.Map<UpdateTodoDto>(todoResult.Value);
            _mapper.Map(partialUpdateDto, updateDto);

            try
            {
                return await UpdateTodo(updateDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Result<GetOneTodoDto>> GetOneTodo(Guid id)
        {
            // check if id is provided
            if (id == Guid.Empty)
            {
                return Result<GetOneTodoDto>.Failure("No ID provided");
            }

            // find existing todo
            var todo = await _dbContext.Todos.FirstOrDefaultAsync(todo => todo.Id == id);

            if (todo == null)
            {
                return Result<GetOneTodoDto>.Failure("Todo not found");
            }

            // return todo
            return Result<GetOneTodoDto>.Success(
                _mapper.Map<GetOneTodoDto>(todo));
        }

        public async Task<Result> UpdateTodo(UpdateTodoDto dto)
        {
            // find existing todo
            var todo = await _dbContext.Todos.FirstOrDefaultAsync(todo => todo.Id == dto.Id);

            if (todo == null)
            {
                return Result.Failure("Todo not found");
            }

            // check for duplicate title
            if (!string.IsNullOrWhiteSpace(dto.Title)
                && dto.Title != todo.Title
                && await TitleAlreadyExists(dto.Title))
            {
                return Result.Failure("A todo with that title already exists");
            }

            // update todo
            _mapper.Map(dto, todo);

            // save to db
            try
            {
                _dbContext.Todos.Update(todo);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbTransactionException();
            }

            // return success
            return Result.Success();
        }

        private async Task<bool> TitleAlreadyExists(string title)
        {
            var existingTodo = await _dbContext.Todos
                .FirstOrDefaultAsync(todo => todo.Title == title);

            return existingTodo != null;
        }
    }
}
