using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TodoProject.Business.Interfaces;
using TodoProject.Data.Interfaces;
using TodoProject.Entities;
using TodoProject.Models;

namespace TodoProject.Business.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;
        private readonly IStringLocalizer<TodoService> _localizer;

        public TodoService(ITodoRepository repository,IStringLocalizer<TodoService> localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }


        public void DeleteTodo(int id, Guid userId)
        {
            _repository.Delete(id, userId);

        }

        public List<TodoItem> GetFilteredTodos(Guid userId, TodoFilterViewModel filter)
        {
            if (!string.IsNullOrEmpty(filter.Category))
            {
                var normalized = filter.Category.Trim().ToLower();

                // Türkçe'den İngilizce'ye çeviri (veritabanı İngilizce kategori saklıyor)
                filter.Category = normalized switch
                {
                    "okul" => "School",
                    "iş" => "Work",
                    "spor" => "Sport",
                    _ => filter.Category // Diğerleri olduğu gibi kalır
                };
            }

            return _repository.GetFilteredTodos(userId, filter);
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
            var testMessage = _localizer["OtherCategoryRequired"];
            if (testMessage.ResourceNotFound)
            {
                Console.WriteLine("Localization key bulunamadı!");
            }
            else
            {
                Console.WriteLine("Localization mesajı: " + testMessage.Value);
            }




            if (item.Category == "Other")
            {
                if (string.IsNullOrWhiteSpace(otherCategory))
                {
                    return (false, _localizer["OtherCategoryRequired"]);
                }
                else if (otherCategory.Length > 20)
                {
                    return (false, _localizer["OtherCategoryMaxLength"]);
                }
                item.Category = otherCategory;
            }

            item.Status = "I"; // Yeni eklenen görev "In Progress" olur
            item.UserId = userId;

            _repository.Add(item);

            return (true, null);
        }

        public (bool isSuccess, string? errorMessage) UpdateTodo(TodoItem updatedItem, string? otherCategory, Guid userId)
        {


            if (updatedItem.Category == "Other")
            {
                if (string.IsNullOrWhiteSpace(otherCategory))
                {
                    return (false,_localizer["OtherCategoryRequired"]);
                }
                else if (otherCategory.Length > 20)
                {
                    return (false, _localizer["OtherCategoryMaxLength"]);
                }

                updatedItem.Category = otherCategory;
            }

            _repository.Update(updatedItem, userId);
            return (true, null);
        }

       

    }
}