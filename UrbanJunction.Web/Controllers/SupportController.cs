using Microsoft.AspNetCore.Mvc;
using UrbanJunction.Data.Models; // Assuming this namespace contains SupportContactFormModel

public class SupportController : Controller
{
    public IActionResult Index()
    {
        return View(); // Looks for Views/Support/Index.cshtml
    }

    [HttpGet]
    public IActionResult Contact()
    {
        return View(); // Optional contact form
    }

    [HttpPost]
    public IActionResult Contact(SupportContactFormModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // TODO: Send email or store feedback
        TempData["Success"] = "Thank you for reaching out! We'll get back to you soon.";
        return RedirectToAction("Index");
    }
}
