using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Escs_Client.Controllers
{
    public class KeyController : Controller
    {

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

    }
}
