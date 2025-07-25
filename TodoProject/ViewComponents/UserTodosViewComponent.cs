using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoProject.Business.Interfaces;
using System.Threading.Tasks;

namespace TodoProject.ViewComponents
{
    public class UserTodosViewComponent : ViewComponent
    {
        private readonly ITodoService _todoService;
        
        public UserTodosViewComponent(ITodoService todoService)
        {
            _todoService = todoService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userIdStr = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (Guid.TryParse(userIdStr, out Guid userId))
            {
                var todos = await _todoService.GetAllTodosAsync(userId);
                return View(todos);
            }

            return View(new List<TodoProject.Entities.TodoItem>());
        }
        
    }
}