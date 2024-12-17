using Escs_Client.Interfaces;
using Escs_Client.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Escs_Client.Controllers
{
    public class AuthenticationController : Controller
    {


        private readonly IAuthenService _authenService;

        private const string BaseUrl = "http://localhost:5212";
        public AuthenticationController(IAuthenService authenService)
        {
            _authenService = authenService;
        }



        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var loginModel = new LoginViewModel();
            return View(loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ModelState.Remove("ReturnUrl");
            if (!ModelState.IsValid)
                return View(model);

            var loginResult = await _authenService.Login(model);
            if (loginResult.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl); // Redirect đến URL trước đó
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Handle login failure
                TempData["ErrorMessage"] = loginResult.ErrorMessage;
                return View(model);
            }


        }




        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string fullName, string email, string phoneNumber, string password, string confirmPassword)
        {
            //if (password != confirmPassword)
            //{
            //    ViewBag.ErrorMessage = "Passwords do not match.";
            //    return View();
            //}

            //var registerCommand = new { FullName = fullName, Email = email, PhoneNumber = phoneNumber, Password = password };

            //var httpCallOptions = HttpCallOptions<dynamic>.UnAuthenticated(
            //    true, false, registerCommand, $"{BaseUrl}/register", new List<string>());

            //var result = await _httpCaller.PostAsync<dynamic, dynamic>(httpCallOptions);

            //if (result.Succeeded)
            //{
            //    return RedirectToAction("Login");
            //}

            //ViewBag.ErrorMessage = "Registration failed.";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _authenService.Logout();

            return RedirectToAction("Login", "Authentication");
        }


    }
}
