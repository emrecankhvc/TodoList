using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoProject.Business.Interfaces;
using TodoProject.Entities;
using TodoProject.Models;
using TodoProject.Business.Interfaces;

namespace TodoProject.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoService _todoService;
        private Guid CurrentUserId => new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        public IActionResult Index(TodoFilterViewModel filter)
        {
            Guid userId = CurrentUserId;
            filter.Items = _todoService.GetFilteredTodos(userId, filter);
            return View(filter);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TodoItem item, string OtherCategory)
        {
            ModelState.Remove("Status");
            ModelState.Remove("OtherCategory");

            if (!ModelState.IsValid)
            {
                return View(item);
            }

            Guid userId = CurrentUserId;

            var (isSuccess, errorMessage) = _todoService.AddTodo(item, OtherCategory, userId);

            if (!isSuccess)
            {
                // Hatalıysa ilgili hata mesajını döndür
                ModelState.AddModelError("OtherCategory", errorMessage ?? "Bir hata oluştu.");
                return View(item);
            }

            TempData["SuccessMessage"] = "Görev başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Guid userId = CurrentUserId;
            var item = _todoService.GetTodoById(id, userId);
            if (item == null) return NotFound();

            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(TodoItem updatedItem, string OtherCategory)
        {
            ModelState.Remove("OtherCategory");

            Guid userId = CurrentUserId;

            var (isSuccess, errorMessage) = _todoService.UpdateTodo(updatedItem, OtherCategory, userId);

            if (!isSuccess)
            {
                // Hatalıysa ilgili hata mesajını döndür
                ModelState.AddModelError("OtherCategory", errorMessage ?? "Bir hata oluştu.");
                ViewData["OtherCategory"] = OtherCategory;
                return View(updatedItem);
            }
            TempData["InfoMessage"] = "Görev güncellendi";
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            Guid userId = CurrentUserId;
            _todoService.DeleteTodo(id, userId);
            TempData["WarningMessage"] = "Görev silindi";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]

        public IActionResult ChangeStatus(int id, string newStatus)
        {
            Guid userId = CurrentUserId;
            bool result = _todoService.ChangeStatus(id, userId, newStatus);

            if (result)
            {
                TempData["ınfoMessage"] = "Görev durumu güncellendi";
            }
            else
            {
                TempData["ErrorMessage"] = "Görev bulunamadı veya geçersiz durum.";
            }

            return RedirectToAction(nameof(Index));
        }




    }
}
