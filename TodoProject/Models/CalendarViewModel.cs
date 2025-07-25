using TodoProject.Entities;

namespace TodoProject.Models
{
    public class CalendarViewModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public int DaysInMonth { get; set; }
        public int FirstDayOfWeek { get; set; }
        public List<CalendarDayViewModel> CalendarDays { get; set; } = new();
    }

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