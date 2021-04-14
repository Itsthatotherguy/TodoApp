using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Models.Attributes;

namespace TodoApp.Models.Todo
{
    public class CreateTodoDto : IEditableTodoDto
    {
        [RequiredDate(ErrorMessage = "Please provide a valid date")]
        public DateTime? DueDate { get; set; }

        [Required(ErrorMessage = "Please provide a title")]
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
