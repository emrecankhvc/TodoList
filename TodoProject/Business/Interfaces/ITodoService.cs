using TodoProject.Entities;
using TodoProject.Models;


namespace TodoProject.Business.Interfaces
{
    public interface ITodoService
    {
        List<TodoItem> GetFilteredTodos(Guid userId, TodoFilterViewModel filter);
        void DeleteTodo(int id,Guid userId);
        TodoItem? GetTodoById(int id,Guid userId);
        bool ChangeStatus(int id, Guid userId, string newStatus);

        (bool isSuccess, string? errorMessage) AddTodo(TodoItem item, string? otherCategory, Guid userId);
        (bool isSuccess, string? errorMessage) UpdateTodo(TodoItem updatedItem, string? otherCategory, Guid userId);

    }
}
