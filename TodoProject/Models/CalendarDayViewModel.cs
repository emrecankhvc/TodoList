using TodoProject.Entities;

namespace TodoProject.Models
{
    public class CalendarDayViewModel
    {
        public int Day { get; set; }
        public DateTime Date { get; set; }
        public bool HasTodos { get; set; }
        public int TodoCount { get; set; }
        public string PriorityColor { get; set; } = "transparent";
        public string StatusColor { get; set; } = "transparent";
        public List<TodoItem> Todos { get; set; } = new();
    }
}
