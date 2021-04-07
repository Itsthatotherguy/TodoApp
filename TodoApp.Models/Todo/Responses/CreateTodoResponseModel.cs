using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Models.Todo.Responses
{
    public record CreateTodoResponseModel
    {
        public Guid Id { get; init; }
    }
}
