using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TodoProject.Models
{
    public class CreateTodoViewModel
    {
        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [StringLength(50, ErrorMessage = "Başlık en fazla 50 karakter olabilir.")]
        public string Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Bitiş tarihi girilmelidir.")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Kategori alanı boş bırakılamaz.")]
        public string? Category { get; set; }
        public bool IsOtherCategory => Category == "Other";
        public string? CustomCategory { get; set; }

        [Required(ErrorMessage = "Öncelik alanı seçilmelidir.")]
        public string Priority { get; set; } 


        public List<SelectListItem>? CategoryList { get; set; }
        public List<SelectListItem>? PriorityList { get; set; }

    }
}
