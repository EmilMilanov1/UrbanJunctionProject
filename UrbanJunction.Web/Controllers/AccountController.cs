using Microsoft.AspNetCore.Mvc;

namespace UrbanJunction.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
