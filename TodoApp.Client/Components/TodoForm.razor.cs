using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TodoApp.Models.Todo;

namespace TodoApp.Client.Components
{
    public partial class TodoForm<TModel> where TModel : IEditableTodoDto
    {
        [Parameter]
        public TModel Model { get; set; }

        [Parameter]
        public EventCallback<EditContext> OnFinish { get; set; }

        [Parameter]
        public bool Loading { get; set; }

        private async void HandleFinish(EditContext editContext)
        {
            await OnFinish.InvokeAsync(editContext);

            StateHasChanged();
        }
    }
}
