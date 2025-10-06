using Microsoft.AspNetCore.Mvc;

namespace UrbanJunction.Web.ViewModels
{
    public class PostFormViewModel
    {
        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public int SubcategoryId { get; set; }
    }
}
