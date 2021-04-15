using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Client.HttpRepository;
using TodoApp.Models.Todo;

namespace TodoApp.Client.Pages
{
    public partial class EditTodo
    {
        // di
        [Inject]
        private ITodoHttpRepository TodoRepository { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        // params
        [Parameter]
        public Guid Id { get; set; }

        // fields
        private UpdateTodoDto _model = new UpdateTodoDto();
        private bool _isLoading = true;
        private List<string> _errors = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            var getResult = await TodoRepository.GetOneTodo(Id);

            if (getResult.IsFailure)
            {
                NavigationManager.NavigateTo("/not-found");
            }

            _model = new UpdateTodoDto
            {
                Title = getResult.Value.Title,
                Description = getResult.Value.Description,
                DueDate = getResult.Value.DueDate
            };

            _isLoading = false;
        }

        private async void OnFinish(EditContext editContext)
        {
            _isLoading = true;

            try
            {
                _model.Id = Id;
                var result = await TodoRepository.UpdateTodo(_model);

                if (result.IsSuccess)
                {
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    _errors = result.Errors;
                }
            }
            catch (Exception)
            {
                _errors = new List<string> { "Something went wrong. Please try again in a while." };
            }

            _isLoading = false;

            StateHasChanged();
        }
    }
}
