using TodoProject.Entities;
using TodoProject.Models;


namespace TodoProject.Business.Interfaces
{
    public interface ITodoService
    {
        List<TodoItem> GetFilteredTodos(Guid userId, TodoFilterViewModel filter);
        void AddTodo(TodoItem item);
        void UpdateTodo(TodoItem item,Guid userId);
        void DeleteTodo(int id,Guid userId);
        TodoItem? GetTodoById(int id,Guid userId);
        bool ChangeStatus(int id, Guid userId, string newStatus);

    }
}
