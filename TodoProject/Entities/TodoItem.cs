using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoProject.Entities
{
    [Table("TodoItems")]
    public class TodoItem
    {
        [Key]
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
            ErrorMessageResourceName = "Category_Required")]
        [StringLength(20,
            ErrorMessageResourceType = typeof(Resources.Models.TodoItem),
            ErrorMessageResourceName = "Category_Length")]
        public string Category { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Resources.Models.TodoItem),
            ErrorMessageResourceName = "DueDate_Required")]
        public DateTime DueDate { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Resources.Models.TodoItem),
            ErrorMessageResourceName = "Status_Required")]
        [StringLength(1)]
        public string Status { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Resources.Models.TodoItem),
            ErrorMessageResourceName = "Priority_Required")]
        [StringLength(10)]
        public string Priority { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}