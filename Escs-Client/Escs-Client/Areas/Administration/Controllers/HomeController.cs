using Microsoft.AspNetCore.Mvc;

namespace Escs_Client.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

    }
}
