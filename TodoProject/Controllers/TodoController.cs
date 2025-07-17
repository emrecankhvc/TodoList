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
        public IActionResult Create(TodoItem item)
        {
            ModelState.Remove("Status");
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
        public IActionResult Edit(TodoItem updatedItem)
        {
            if (ModelState.IsValid)
            {
                Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                _todoService.UpdateTodo(updatedItem, userId);
                TempData["InfoMessage"] = "Görev güncellendi";
                return RedirectToAction(nameof(Index));
            }

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
    }
}
