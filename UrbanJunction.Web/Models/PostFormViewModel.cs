using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace UrbanJunction.Web.Models
{
    public class PostFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Content cannot be empty.")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; } = null!;

        [Required(ErrorMessage = "Please select a subcategory.")]
        [Display(Name = "Subcategory")]
        public int SubcategoryId { get; set; }

        public List<SelectListItem>? Subcategories { get; set; }

        [Display(Name = "Upload Images")]
        public List<IFormFile>? ImageFiles { get; set; }
    }
}
