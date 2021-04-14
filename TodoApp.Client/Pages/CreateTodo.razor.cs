using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TodoApp.Client.HttpRepository;
using TodoApp.Models.Todo.Requests;

namespace TodoApp.Client.Pages
{
    public partial class CreateTodo
    {
        [Inject]
        private ITodoHttpRepository TodoRepository { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private CreateTodoRequestModel _model = new CreateTodoRequestModel();        

        public bool IsLoading { get; set; } = false;

        private bool _isLoading = false;
        private List<string> _errors = new List<string>();

        private async void OnFinish(EditContext editContext)
        {
            _isLoading = true;

            await Task.Delay(2000);

            var result = await TodoRepository.CreateTodo(_model);

            _isLoading = false;            

            if (result.IsSuccess)
            {
                NavigationManager.NavigateTo("/");
            }
            else
            {
                _errors = result.Errors;
            }

            StateHasChanged();
        }
    }
}
