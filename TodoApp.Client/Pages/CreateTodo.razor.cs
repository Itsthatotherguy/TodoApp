using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using TodoApp.Client.HttpRepository;
using TodoApp.Models.Todo;

namespace TodoApp.Client.Pages
{
    public partial class CreateTodo
    {
        // di
        [Inject]
        private ITodoHttpRepository TodoRepository { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        // fields
        private CreateTodoDto _model = new CreateTodoDto();
        private bool _isLoading = false;
        private List<string> _errors = new List<string>();

        private async void OnFinish(EditContext editContext)
        {
            _isLoading = true;

            try
            {
                var result = await TodoRepository.CreateTodo(_model);

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
