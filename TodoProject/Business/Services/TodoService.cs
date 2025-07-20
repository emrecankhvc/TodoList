using Microsoft.EntityFrameworkCore;
using TodoProject.Business.Interfaces;
using TodoProject.Data.Interfaces;  
using TodoProject.Entities;
using TodoProject.Models;

namespace TodoProject.Business.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository repository)
        {
            _repository = repository;

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

        public (bool isSuccess, string? errorMessage) AddTodo(TodoItem item, string? otherCategory, Guid userId)
        {


            if (item.Category == "Other")
            {
                if (string.IsNullOrWhiteSpace(otherCategory))
                {
                    return (false, "Lütfen diğer kategoriyi giriniz.");
                }
                else if (otherCategory.Length > 20)
                {
                    return (false, "Diğer kategori en fazla 20 karakter olabilir.");
                }
                item.Category = otherCategory;
            }
            item.UserId = userId;
            item.Status = "I";
            _repository.Add(item);
            return (true, null);
        }

        public (bool isSuccess, string? errorMessage) UpdateTodo(TodoItem updatedItem, string? otherCategory, Guid userId)
        {
            if (updatedItem.Category == "Other")
            {
                if (string.IsNullOrWhiteSpace(otherCategory))
                {
                    return (false, "Lütfen diğer kategoriyi giriniz.");
                }
                else if (otherCategory.Length > 20)
                {
                    return (false, "Diğer kategori en fazla 20 karakter olabilir.");
                }

                updatedItem.Category = otherCategory;
            }

            _repository.Update(updatedItem, userId);
            return (true, null);
        }

    }
}
