using TodoProject.Entities;

namespace TodoProject.Models
{
    public class EditTodoViewModel
    {
        public TodoItem Item { get; set; } = new();
        public string? Status { get; set; }
    }
}
