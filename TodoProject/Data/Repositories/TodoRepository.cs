using Microsoft.EntityFrameworkCore;
using TodoProject.Data.Interfaces;
using TodoProject.Entities;
using TodoProject.Models;

namespace TodoProject.Data.Repositories
{

    public class TodoRepository : ITodoRepository
    {
        private readonly DatabaseContext _context;

        public TodoRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Add(TodoItem item)
        {
            _context.TodoItems.Add(item);
            _context.SaveChanges();
        }

        public void Delete(int id, Guid userId)
        {
            var item = _context.TodoItems.FirstOrDefault(x => x.Id == id && x.UserId == userId);
            if (item != null)
            {
                _context.TodoItems.Remove(item);
                _context.SaveChanges();
            }
        }

        public List<TodoItem> GetFilteredTodos(Guid userId, TodoFilterViewModel filter)
        {
            var query = _context.TodoItems.Where(x => x.UserId == userId);
            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(x => x.Status.ToUpper() == filter.Status.ToUpper());
            if (!string.IsNullOrEmpty(filter.Priority))
                query = query.Where(x => x.Priority.ToLower() == filter.Priority.ToLower());
            if (!string.IsNullOrEmpty(filter.Category))
                query = query.Where(x => x.Category.ToLower() == filter.Category.ToLower());
            query = filter.SortBy switch
            {
                "duedate_asc" => query.OrderBy(x => x.DueDate),
                "duedate_desc" => query.OrderByDescending(x => x.DueDate),
                "status" => query.OrderBy(x => x.Status),
                "category" => query.OrderBy(x => x.Category),
                "priority" => query.OrderByDescending(x =>
                    x.Priority == "High" ? 3 :
                    x.Priority == "Medium" ? 2 :
                    x.Priority == "Low" ? 1 : 0),
                "title" => query.OrderBy(x => x.Title),
                _ => query.OrderByDescending(x => x.DueDate)
            };
            return query.ToList();
        }

        public void Update(TodoItem updatedItem, Guid userId)
        {
            var item = _context.TodoItems.FirstOrDefault(x => x.Id == updatedItem.Id && x.UserId == userId);
            if (item != null)
            {
                item.Title = updatedItem.Title;
                item.Description = updatedItem.Description;
                item.Category = updatedItem.Category;
                item.DueDate = updatedItem.DueDate;
                item.Priority = updatedItem.Priority;
                item.Status = updatedItem.Status;
                _context.SaveChanges();
            }

        }

        public TodoItem? GetById(int id, Guid userId)
        {
            return _context.TodoItems.FirstOrDefault(x => x.Id == id && x.UserId == userId);


        }

        public async Task<List<TodoItem>> GetAllTodosAsync(Guid userId)
        { 
            return await _context.TodoItems
                .Where(x => x.UserId == userId)
                .OrderByDescending(t => t.DueDate)
                .ToListAsync();
        }



        public List<TodoItem> GetTodosByDateRange(Guid userId, DateTime startDate, DateTime endDate)
        {
            return _context.TodoItems
                .Where(x => x.UserId == userId &&
                           x.DueDate.Date >= startDate.Date &&
                           x.DueDate.Date <= endDate.Date)
                .OrderBy(x => x.DueDate)
                .ToList();
        }

        public List<TodoItem> GetTodosByDate(Guid userId, DateTime date)
        {
            return _context.TodoItems
                .Where(x => x.UserId == userId && x.DueDate.Date == date.Date)
                .OrderBy(x => x.DueDate)
                .ToList();
        }
    }
}
