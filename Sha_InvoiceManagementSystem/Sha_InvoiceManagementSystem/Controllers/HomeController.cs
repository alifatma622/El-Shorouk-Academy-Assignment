using Microsoft.AspNetCore.Mvc;

namespace Sha_InvoiceManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
