using TodoProject.Entities;
using TodoProject.Models;

namespace TodoProject.Data.Interfaces
{
    public interface ITodoRepository
    {
        void Add(TodoItem item);
        void Delete(int id, Guid userId);
        List<TodoItem> GetFilteredTodos(Guid userId, TodoFilterViewModel filter);
        TodoItem? GetById(int id, Guid userId);
        void Update(TodoItem item, Guid userId);

        Task<List<TodoItem>> GetAllTodosAsync(Guid userId);
    }
}
