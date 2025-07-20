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

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        public IActionResult Index(TodoFilterViewModel filter)
        {
            Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
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

            if (item.Category == "Other")
            {
                if (string.IsNullOrWhiteSpace(OtherCategory))
                {
                    ModelState.AddModelError("OtherCategory", "Lütfen diğer kategoriyi giriniz.");
                }
                else if (OtherCategory.Length > 20)
                {
                    ModelState.AddModelError("OtherCategory", "Diğer kategori en fazla 20 karakter olabilir.");
                }
                else
                {
                    item.Category = OtherCategory;
                }
            }


            if (ModelState.IsValid)
            {

                item.UserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                _todoService.AddTodo(item);
                TempData["SuccessMessage"] = "Görev eklendi";
                return RedirectToAction(nameof(Index));
            }

            return View(item);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var item = _todoService.GetTodoById(id, userId);
            if (item == null) return NotFound();

            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(TodoItem updatedItem, string OtherCategory)
        {
            ModelState.Remove("OtherCategory");
            if (updatedItem.Category == "Other")
            {
                if (string.IsNullOrWhiteSpace(OtherCategory))
                {
                    ModelState.AddModelError("OtherCategory", "Lütfen diğer kategoriyi giriniz.");
                }
                else if (OtherCategory.Length > 20)
                {
                    ModelState.AddModelError("OtherCategory", "Diğer kategori en fazla 20 karakter olabilir.");
                }
                else
                {
                    updatedItem.Category = OtherCategory;
                }
            }

            if (ModelState.IsValid)


            {
                Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                _todoService.UpdateTodo(updatedItem, userId);
                TempData["InfoMessage"] = "Görev güncellendi";
                return RedirectToAction(nameof(Index));
            }
            ViewData["OtherCategory"] = OtherCategory;
            return View(updatedItem);
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _todoService.DeleteTodo(id, userId);
            TempData["WarningMessage"] = "Görev silindi";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]

        public IActionResult ChangeStatus(int id, string newStatus)
        {
            Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
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
