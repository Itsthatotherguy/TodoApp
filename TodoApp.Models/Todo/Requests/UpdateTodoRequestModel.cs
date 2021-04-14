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
    public class UpdateTodoRequestModel
    {
        [Required]
        public Guid Id { get; set; }

        [RequiredDate]
        public DateTime? DueDate { get; set; }

        [Required(ErrorMessage = "Please provide a title")]
        public string Title { get; set; }

        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
