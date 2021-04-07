using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Models.Todo.Requests
{
    public record UpdateTodoRequestModel
    {
        public Guid Id { get; init; }
        public DateTime DueDate { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public bool IsCompleted { get; init; }
    }

    public class UpdateTodoRequestModelValidator : AbstractValidator<UpdateTodoRequestModel>
    {
        public UpdateTodoRequestModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
