using TodoProject.Business.Interfaces;
using TodoProject.Data.Interfaces;
using TodoProject.Entities;
using TodoProject.Models;

namespace TodoProject.Business.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly ITodoRepository _todoRepository;

        public CalendarService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public CalendarViewModel GetCalendarData(Guid userId, int year, int month)
        {
            // Seçilen ayın başlangıç ve bitiş tarihlerini hesapla
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            // O aydaki tüm görevleri getir
            var todos = _todoRepository.GetTodosByDateRange(userId, startDate, endDate);

            var calendarViewModel = new CalendarViewModel
            {
                Year = year,
                Month = month,
                MonthName = startDate.ToString("MMMM yyyy"),
                DaysInMonth = DateTime.DaysInMonth(year, month),
                FirstDayOfWeek = (int)startDate.DayOfWeek,
                CalendarDays = new List<CalendarDayViewModel>()
            };

            // Her gün için veri hazırla
            for (int day = 1; day <= calendarViewModel.DaysInMonth; day++)
            {
                var currentDate = new DateTime(year, month, day);
                var dayTodos = todos.Where(t => t.DueDate.Date == currentDate.Date).ToList();

                var calendarDay = new CalendarDayViewModel
                {
                    Day = day,
                    Date = currentDate,
                    HasTodos = dayTodos.Any(),
                    TodoCount = dayTodos.Count,
                    PriorityColor = GetDayPriorityColor(dayTodos),
                    StatusColor = GetDayStatusColor(dayTodos),
                    Todos = dayTodos
                };

                calendarViewModel.CalendarDays.Add(calendarDay);
            }

            return calendarViewModel;
        }

        private string GetDayPriorityColor(List<TodoItem> dayTodos)
        {
            if (!dayTodos.Any()) return "transparent";

            // En yüksek önceliğe göre renk belirle
            if (dayTodos.Any(t => t.Priority == "High")) return "#dc3545"; // Kırmızı
            if (dayTodos.Any(t => t.Priority == "Medium")) return "#ffc107"; // Sarı
            if (dayTodos.Any(t => t.Priority == "Low")) return "#28a745"; // Yeşil

            return "#6c757d"; // Gri
        }

        private string GetDayStatusColor(List<TodoItem> dayTodos)
        {
            if (!dayTodos.Any()) return "transparent";

            var completedCount = dayTodos.Count(t => t.Status == "C");
            var totalCount = dayTodos.Count;

            // Tamamlanma durumuna göre renk yoğunluğu
            if (completedCount == totalCount) return "#28a745"; // Hepsi tamamlandı - Yeşil
            if (completedCount > 0) return "#ffc107"; // Kısmen tamamlandı - Sarı
            return "#dc3545"; // Hiçbiri tamamlanmadı - Kırmızı
        }

        public List<TodoItem> GetTodosByDate(Guid userId, DateTime date)
        {
            return _todoRepository.GetTodosByDate(userId, date);
        }
    }
}