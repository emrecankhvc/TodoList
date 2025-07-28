using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TodoProject.Models
{
    public class EditTodoViewModel
    {
        public int Id { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Resources.Models.TodoItem),
            ErrorMessageResourceName = "Title_Required")]
        [StringLength(50,
            ErrorMessageResourceType = typeof(Resources.Models.TodoItem),
            ErrorMessageResourceName = "Title_Length")]
        public string Title { get; set; }

        [StringLength(500,
            ErrorMessageResourceType = typeof(Resources.Models.TodoItem),
            ErrorMessageResourceName = "Description_Length")]
        public string? Description { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Resources.Models.TodoItem),
            ErrorMessageResourceName = "DueDate_Required")]
        public DateTime DueDate { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Resources.Models.TodoItem),
            ErrorMessageResourceName = "Category_Required")]
        [StringLength(20,
            ErrorMessageResourceType = typeof(Resources.Models.TodoItem),
            ErrorMessageResourceName = "Category_Length")]
        public string? Category { get; set; }

        public bool IsOtherCategory => Category == "Other";

        public string? CustomCategory { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Resources.Models.TodoItem),
            ErrorMessageResourceName = "Priority_Required")]
        [StringLength(10)]
        public string Priority { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Resources.Models.TodoItem),
            ErrorMessageResourceName = "Status_Required")]
        [StringLength(1)]
        public string Status { get; set; }

        public List<SelectListItem>? CategoryList { get; set; }
        public List<SelectListItem>? PriorityList { get; set; }
        public List<SelectListItem>? StatusList { get; set; }
    }
}