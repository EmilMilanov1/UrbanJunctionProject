using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


public class PostFormViewModel
{
    public int Id { get; set; } // needed for Edit

    [Required]
    [StringLength(150)]
    public string Title { get; set; }

    [Required]
    [StringLength(4000)]
    public string Content { get; set; }

    [Display(Name = "Subcategory")]
    [Required]
    public int SubcategoryId { get; set; }

    public IEnumerable<SelectListItem>? Subcategories { get; set; } // for dropdown list
}
