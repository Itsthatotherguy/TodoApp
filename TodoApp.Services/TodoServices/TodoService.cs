using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Database.Contexts;
using TodoApp.Domain.Entities;
using TodoApp.Models.Todo.Requests;
using TodoApp.Models.Todo.Responses;
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

        public async Task<Result<CreateTodoResponseModel>> CreateTodo(CreateTodoRequestModel model)
        {
            // validate model
            var modelValidation = new CreateTodoRequestModelValidator().Validation(model);

            if (modelValidation.IsFailure)
            {
                return Result<CreateTodoResponseModel>.Failure(modelValidation.Errors);
            }

            // check that todo with the same title doesnt exist
            if (await TitleAlreadyExists(model.Title))
            {
                return Result<CreateTodoResponseModel>.Failure("A todo with that title already exists");
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
            return Result<CreateTodoResponseModel>.Success(
                _mapper.Map<CreateTodoResponseModel>(todo));
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

        public async Task<Result<IEnumerable<ListTodosResponseModel>>> ListTodos()
        {
            var todoList = await _dbContext.Todos.ToListAsync();

            if (todoList == null)
            {
                throw new DbTransactionException();
            }

            return Result<IEnumerable<ListTodosResponseModel>>.Success(
                _mapper.Map<IEnumerable<ListTodosResponseModel>>(todoList));
        }

        public async Task<Result<ReadTodoResponseModel>> ReadTodo(Guid id)
        {
            // check if id is provided
            if (id == Guid.Empty)
            {
                return Result<ReadTodoResponseModel>.Failure("No ID provided");
            }

            // find existing todo
            var todo = await _dbContext.Todos.FirstOrDefaultAsync(todo => todo.Id == id);

            if (todo == null)
            {
                return Result<ReadTodoResponseModel>.Failure("Todo not found");
            }

            // return todo
            return Result<ReadTodoResponseModel>.Success(
                _mapper.Map<ReadTodoResponseModel>(todo));
        }

        public async Task<Result> UpdateTodo(UpdateTodoRequestModel model)
        {
            // model validation
            var modelValidation = new UpdateTodoRequestModelValidator().Validation(model);

            if (modelValidation.IsFailure)
            {
                return Result.Failure(modelValidation.Errors);
            }

            // find existing todo
            var todo = await _dbContext.Todos.FirstOrDefaultAsync(todo => todo.Id == model.Id);

            if (todo == null)
            {
                return Result.Failure("Todo not found");
            }

            // check for duplicate title
            if (model.Title != null && await TitleAlreadyExists(model.Title))
            {
                return Result.Failure("A todo with that title already exists");
            }

            // update todo
            _mapper.Map(model, todo);

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
