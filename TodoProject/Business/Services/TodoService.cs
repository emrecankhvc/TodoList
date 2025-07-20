using TodoProject.Entities;
using TodoProject.Models;
using TodoProject.Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using TodoProject.Data.Interfaces;  

namespace TodoProject.Business.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository repository)
        {
            _repository = repository;

        }

        public void AddTodo(TodoItem item)
        {
            item.Status = "I";
            _repository.Add(item);
        }

        public void DeleteTodo(int id, Guid userId)
        {
            _repository.Delete(id, userId);

        }

        public List<TodoItem> GetFilteredTodos(Guid userId, TodoFilterViewModel filter)
        {
            return _repository.GetFilteredTodos(userId,filter);
        }
        public TodoItem? GetTodoById(int id, Guid userId)
        {
            return _repository.GetById(id, userId);
        }

        public void UpdateTodo(TodoItem updatedItem, Guid userId)
        {
            _repository.Update(updatedItem, userId);
        }

        public bool ChangeStatus(int id, Guid userId, string newStatus)
        {
            var todoItem = _repository.GetById(id, userId);
            if (todoItem != null && (newStatus == "C" || newStatus == "U"))
            {
                todoItem.Status = newStatus;
                _repository.Update(todoItem, userId);
                return true;
            }
            return false;
        }

    }
}
