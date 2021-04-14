using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Client.HttpRepository;
using TodoApp.Models.Todo.Responses;

namespace TodoApp.Client.Components
{
    public partial class TodoList
    {
        [Parameter]
        public List<ListTodosResponseModel> Todos { get; set; }
    }
}
