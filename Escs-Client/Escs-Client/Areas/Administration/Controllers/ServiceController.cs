using Escs_Client.Interfaces;
using Escs_Client.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Escs_Client.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class ServiceController : Controller
    {

        private readonly IEmailService _emailService;
        private readonly IEndpointService _endpointService;
        private readonly IKeyService _keyService;
        private readonly IConfiguration _configuration;

        public ServiceController(IEmailService emailService, IEndpointService endpointService, IKeyService keyService, IConfiguration configuration)
        {
            _emailService = emailService;
            _endpointService = endpointService;
            _keyService = keyService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Email()
        {
            // Retrieve the email index from the configuration
            var emailIndex = _configuration.GetSection("ServiceIndex:Email").Value;

            // Pass the email index to the service or directly to the ViewBag
            ViewBag.EmailIndex = emailIndex;
            var endpoints = await _emailService.GetEndpointOfEmailService();
            return View(endpoints.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEndpoint(CreateEndpointRequest request)
        {
            if (ModelState.IsValid)
            {
                request.ServiceId = 1;
                var result = await _endpointService.CreateEndpoint(request);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Endpoint created successfully.";
                }
                else
                {
                    TempData["Error"] = result.ErrorMessage;
                }
            }
            return RedirectToAction("Email");
        }

        [HttpGet]
        public async Task<IActionResult> GetEndpointById(long id)
        {
            var result = await _endpointService.GetEndpointById(id);
            if (result.Succeeded)
            {
                return PartialView("_UpdateEndpointPartial", result.Data);
            }
            TempData["Error"] = "Could not fetch endpoint details.";
            return RedirectToAction("Email");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEndpoint(UpdateEndpointRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _endpointService.UpdateEndpoint(request);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Endpoint updated successfully.";
                }
                else
                {
                    TempData["Error"] = result.ErrorMessage;
                }
            }
            return RedirectToAction("Email");
        }


        [HttpGet]
        public async Task<IActionResult> GetUpdateModal(long id)
        {
            // Fetch the endpoint data from the service or database
            var endpoint = await _endpointService.GetEndpointById(id);
            if (endpoint == null)
            {
                return NotFound("Endpoint not found.");
            }

            // Return the partial view with the model
            return PartialView("_UpdateEndpointPartial", endpoint.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserInEmailService(UserSearchViewModel userSearchViewModel)
        {

            if (userSearchViewModel.PageSize is null || userSearchViewModel.PageSize < 1)
            {
                userSearchViewModel.PageSize = 5;
            }

            var result = await _emailService.GetUserInEmailServiceWithPaging(userSearchViewModel);

            if (result.Succeeded)
            {
                // Pass search parameters through ViewData
                ViewData["UserName"] = userSearchViewModel.UserName;
                ViewData["PageIndex"] = userSearchViewModel.PageIndex;
                ViewData["PageSize"] = userSearchViewModel.PageSize;

                return View(result.Data); // Pass the response to the view
            }

            ModelState.AddModelError(string.Empty, "Failed to fetch user data.");
            return View(new ItemPagingResponse<UserResponse>()); // Return an empty model in case of failure
        }

        [HttpGet]
        public async Task<IActionResult> GetUserRequest(long userId)
        {

            var serviceRegisterByUserResult = await _keyService.GetServiceEndpointRegisterByUser(userId);

            ViewBag.UserId = userId;
            ViewBag.Endpoints = serviceRegisterByUserResult.Data;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetEndpointUsage(string endpoint)
        {
            var endpointSplit = endpoint.Split('-');

            ViewBag.Url = endpointSplit[0];
            ViewBag.Method = endpointSplit[1];
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> SearchLogs(string index)
        {
            ViewBag.Index = index;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RequestLogs(string index)
        {
            ViewBag.Index = index;
            return View();
        }
    }
}
