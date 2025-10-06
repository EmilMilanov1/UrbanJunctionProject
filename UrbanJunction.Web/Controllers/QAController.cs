using Microsoft.AspNetCore.Mvc;

namespace UrbanJunction.Web.Controllers
{
    public class QAController : Controller
    {
        public IActionResult Index()
        {
            return View(); // Looks for Views/QA/Index.cshtml
        }
    }
}
