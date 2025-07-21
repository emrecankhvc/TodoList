using Microsoft.AspNetCore.Mvc.Rendering;
using TodoProject.Entities;

namespace TodoProject.Models
{
    public class EditTodoViewModel
    {
        public Guid Id { get; set; } // Görevin kimliği
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }

        public string? Category { get; set; }
        public bool IsOtherCategory { get; set; }
        public string? CustomCategory { get; set; }

        public List<SelectListItem>? CategoryList { get; set; }
    }
}