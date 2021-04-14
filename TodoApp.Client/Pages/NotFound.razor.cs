using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp.Client.Pages
{
    public partial class NotFound
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private void NavigateHome()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
