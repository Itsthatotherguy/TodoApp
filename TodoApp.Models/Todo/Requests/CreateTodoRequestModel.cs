using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Models.Todo.Requests
{
    public record CreateTodoRequestModel
    {
        public DateTime DueDate { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
    }

    public class CreateTodoRequestModelValidator : AbstractValidator<CreateTodoRequestModel>
    {
        public CreateTodoRequestModelValidator()
        {
            RuleFor(x => x.DueDate)
                .NotEmpty();

            RuleFor(x => x.Title)
                .NotEmpty();
        }
    }
}
