using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Models.Todo
{
    public interface IEditableTodoDto
    {
        DateTime? DueDate { get; set; }

        string Title { get; set; }

        string Description { get; set; }
    }
}
