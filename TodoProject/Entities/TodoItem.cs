using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoProject.Entities
{
    [Table("TodoItems")]
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [StringLength(50,ErrorMessage = "Başlık en fazla 50 karakter olabilir.")]
        public string Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Kategori alanı boş bırakılamaz.")]
        [StringLength(20,ErrorMessage = "Kategori en fazla 20 karakter olabilir.")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Bitiş tarihi girilmelidir.")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Durum alanı seçilmelidir.")]
        [StringLength(1)]
        public string Status { get; set; } // "U" (Uncompleted), "I" (In Progress), "C" (Completed)

        [Required(ErrorMessage = "Öncelik alanı seçilmelidir.")]
        [StringLength(10)]
        public string Priority { get; set; } // "Low", "Medium", "High"

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}