using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Client.HttpRepository;
using TodoApp.Models.Todo.Requests;
using TodoApp.Models.Todo.Responses;
using TodoApp.Utilities;

namespace TodoApp.Client.Pages
{
    public partial class Index
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ITodoHttpRepository TodoRepository { get; set; }

        private List<ListTodosResponseModel> _todoList { get; set; } = new List<ListTodosResponseModel>();
        private bool _isLoading { get; set; } = true;
        private List<string> _errors = new List<string>();
        private Guid _editId;
        //private string _editCellName;
        //private ListTodosResponseModel _previousTodoInfo;
        IDictionary<Guid, (bool edit, ListTodosResponseModel model)> _editCache = new Dictionary<Guid, (bool edit, ListTodosResponseModel model)>();

        //private void StartEdit(Guid id, string editCellName)
        //{
        //    _previousTodoInfo = _todoList.FirstOrDefault(todo => todo.Id == id).DeepCopy();

        //    _editId = id;
        //    _editCellName = editCellName;
        //}

        //private async Task StopEdit()
        //{
        //    var result = await UpdateTodo();

        //    if (result.IsSuccess)
        //    {                
        //        _editCellName = null;
        //    }
        //    else
        //    {
        //        _errors = result.Errors;
        //    }

        //    StateHasChanged();
        //}

        private async Task<Result> UpdateTodo(Guid id)
        {
            _isLoading = true;

            var todo = GetTodo(id);

            var model = new UpdateTodoRequestModel
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

        private void StartEdit(Guid id)
        {
            _editId = id;
            var data = _editCache[id];
            data.edit = true;
            _editCache[id] = data;
        }

        private void CancelEdit(Guid id)
        {
            var data = GetTodo(id);
            _editCache[id] = new(false, data);
        }

        private void SaveEditKeyboard(KeyboardEventArgs eventArgs)
        {
            if (eventArgs.Key == "Enter")
            {
                SaveEdit(_editId);
            }
        }

        private async void SaveEdit(Guid id)
        {
            var result = await UpdateTodo(id);

            _editCache[id] = new(false, GetTodo(id));

            if (result.IsFailure)
            {
                _errors = result.Errors;
            }

            StateHasChanged();
        }

        private void UpdateEditCache()
        {
            _todoList.ForEach(todo =>
            {
                _editCache[todo.Id] = new(false, todo);
            });
        }

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

        private void NavigateToCreateTodo()
        {
            NavigationManager.NavigateTo("/createTodo");
        }

        protected async override Task OnInitializedAsync()
        {
            var result = await TodoRepository.GetTodos();

            _todoList = result.Value;

            UpdateEditCache();

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

        private ListTodosResponseModel GetTodo(Guid id) => _todoList.FirstOrDefault(todo => todo.Id == id);
    }
}
