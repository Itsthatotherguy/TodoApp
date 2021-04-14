using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Models.Attributes;

namespace TodoApp.Models.Todo.Requests
{
    public class CreateTodoRequestModel
    {
        [RequiredDate]
        public DateTime? DueDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Please provide a title")]
        public string Title { get; set; }


        public string Description { get; set; }
    }

    public class CreateTodoRequestModelValidator : AbstractValidator<CreateTodoRequestModel>
    {
        public CreateTodoRequestModelValidator()
        {
            //RuleFor(x => x.DueDate)
            //    .NotEmpty()
            //    .WithMessage("Please specify a due date");

            //RuleFor(x => x.Title)
            //    .NotEmpty()
            //    .WithMessage("Please specify a title");
        }
    }
}
