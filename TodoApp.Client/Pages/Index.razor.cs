using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Client.HttpRepository;
using TodoApp.Models.Todo;
using TodoApp.Utilities;

namespace TodoApp.Client.Pages
{
    public partial class Index
    {
        // di
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ITodoHttpRepository TodoRepository { get; set; }

        // fields
        private List<GetAllTodosDto> _todoList { get; set; } = new List<GetAllTodosDto>();
        private bool _isLoading { get; set; } = true;
        private List<string> _errors = new List<string>();

        // methods
        private async Task<Result> UpdateTodo(Guid id)
        {
            _isLoading = true;

            var todo = GetTodo(id);

            var model = new UpdateTodoDto
            {
                Id = id,
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                DueDate = todo.DueDate
            };

            var result = await TodoRepository.UpdateTodo(model);

            _isLoading = false;

            return result.IsSuccess ? Result.Success() : Result.Failure(result.Errors);
        }

        private async void ToggleCompleted(Guid id)
        {
            var result = await UpdateTodo(id);

            if (result.IsFailure)
            {
                _errors = result.Errors;
            }

            StateHasChanged();
        }

        private string FormatDate(DateTime? date) => ((DateTime)date).ToString("dd MMMM yyyy") ?? null;

        private async void DeleteTodo(Guid id)
        {
            _isLoading = true;

            var result = await TodoRepository.DeleteTodo(id);

            if (result.IsFailure)
            {
                _errors = result.Errors;
            }

            _todoList.RemoveAll(todo => todo.Id == id);

            _isLoading = false;

            StateHasChanged();
        }

        private async void MarkAsCompleted(Guid id)
        {
            _isLoading = true;

            var result = await TodoRepository.MarkTodoComplete(id);

            if (result.IsSuccess)
            {
                var todo = GetTodo(id);
                todo.IsCompleted = true;
            }
            else
            {
                _errors = result.Errors;
            }

            _isLoading = false;

            StateHasChanged();
        }

        private async void MarkAsIncomplete(Guid id)
        {
            _isLoading = true;

            var result = await TodoRepository.MarkTodoIncomplete(id);

            if (result.IsSuccess)
            {
                var todo = GetTodo(id);
                todo.IsCompleted = false;
            }
            else
            {
                _errors = result.Errors;
            }

            _isLoading = false;

            StateHasChanged();
        }

        private void NavigateToCreateTodo()
        {
            NavigationManager.NavigateTo("/createTodo");
        }

        private void NavigateToEditTodo(Guid id)
        {
            NavigationManager.NavigateTo($"/editTodo/{id.ToString()}");
        }

        protected async override Task OnInitializedAsync()
        {
            try
            {
                var result = await TodoRepository.GetTodos();

                _todoList = result.Value;
            }
            catch (Exception)
            {
                _todoList = new List<GetAllTodosDto>();
                _errors = new List<string> { "Something went wrong. Please try again in a while." };
            }

            _isLoading = false;
        }

        //private bool TodoHasChanged(ListTodosResponseModel todo)
        //{
        //    var newTodoValue = GetPropertyValue(todo, _editCellName);
        //    var oldTodoValue = GetPropertyValue(_previousTodoInfo, _editCellName);

        //    return newTodoValue != oldTodoValue;
        //}

        //private object GetPropertyValue(object instance, string propertyName)
        //{
        //    var type = instance.GetType();
        //    var propertyInfo = type.GetProperty(propertyName);

        //    return propertyInfo.GetValue(instance, null);
        //}

        private GetAllTodosDto GetTodo(Guid id) => _todoList.FirstOrDefault(todo => todo.Id == id);
    }
}
