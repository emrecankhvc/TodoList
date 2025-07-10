using TodoProject.Entities;

namespace TodoProject.Models
{
    public class TodoFilterViewModel
    {
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public string? Category { get; set; }
        public string? SortBy { get; set; }
        public List<TodoItem> Items { get; set; } = new();
    }

}