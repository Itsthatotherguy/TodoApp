using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Models.Todo
{
    public class UpdateTodoDto : IEditableTodoDto
    {
        [Required]
        public Guid Id { get; set; }

        public DateTime? DueDate { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
