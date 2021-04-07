using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Models.Todo.Responses
{
    public record ListTodosResponseModel
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public bool IsCompleted { get; init; }
    }
}
