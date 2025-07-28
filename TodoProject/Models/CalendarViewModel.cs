

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

    
}