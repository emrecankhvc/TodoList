using TodoProject.Entities;
using TodoProject.Models;

namespace TodoProject.Business.Interfaces
{
    public interface ICalendarService
    {
        CalendarViewModel GetCalendarData(Guid userId, int year, int month);
        List<TodoItem> GetTodosByDate(Guid userId, DateTime date);
    }
}