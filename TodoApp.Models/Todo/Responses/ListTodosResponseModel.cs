using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Models.Todo.Responses
{
    [Serializable]
    public class ListTodosResponseModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }

        public ListTodosResponseModel DeepCopy()
        {
            return new ListTodosResponseModel
            {
                Id = Id,
                Title = Title,
                Description = Description,
                IsCompleted = IsCompleted
            };
        }
    }
}
