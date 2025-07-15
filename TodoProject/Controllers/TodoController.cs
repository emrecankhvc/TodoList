using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoProject.Entities;
using TodoProject.Models;

namespace TodoProject.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly DatabaseContext _context;

        public TodoController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index(TodoFilterViewModel filter)
        {
            Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
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
                
                   x.Priority == "High" ? 3:
                   x.Priority == "Medium" ? 2:
                   x.Priority == "Low" ? 1:0),

                "title" => query.OrderBy(x => x.Title),
                _ => query.OrderByDescending(x => x.DueDate)
            };

            filter.Items = query.ToList();

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
                item.Status = "I";
                _context.TodoItems.Add(item);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Görev eklendi";
                return RedirectToAction(nameof(Index));
            }

            return View(item);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var item = _context.TodoItems.FirstOrDefault(x => x.Id == id && x.UserId == userId);
            if (item == null) return NotFound();

            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(TodoItem updatedItem)
        {
            if (ModelState.IsValid)
            {
                Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var item = _context.TodoItems.FirstOrDefault(x => x.Id == updatedItem.Id && x.UserId == userId);

                if (item == null) return NotFound();

                item.Title = updatedItem.Title;
                item.Description = updatedItem.Description;
                item.Category = updatedItem.Category;
                item.DueDate = updatedItem.DueDate;
                item.Priority = updatedItem.Priority;
                item.Status = updatedItem.Status;

                _context.SaveChanges();
                TempData["InfoMessage"] = "Görev güncellendi";
                return RedirectToAction(nameof(Index));
            }

            return View(updatedItem);
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var item = _context.TodoItems.FirstOrDefault(x => x.Id == id && x.UserId == userId);
            if (item == null) return NotFound();

            _context.TodoItems.Remove(item);
            _context.SaveChanges();
            TempData["WarningMessage"] = "Görev silindi";
            return RedirectToAction(nameof(Index));
        }
    }
}
