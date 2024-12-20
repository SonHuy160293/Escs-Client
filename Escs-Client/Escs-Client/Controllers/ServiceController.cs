using Escs_Client.Interfaces;
using Escs_Client.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Escs_Client.Controllers
{
    public class ServiceController : Controller
    {

        private readonly IEmailService _emailService;
        private readonly IKeyService _keyService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ServiceController(IEmailService emailService, IHttpContextAccessor httpContextAccessor, IKeyService keyService)
        {
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _keyService = keyService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Email()
        {

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                var userEmailConfigs = await _emailService.GetEmailConfigurationByUserId(Int64.Parse(userId));

                if (userEmailConfigs.Data.Any())
                {
                    return RedirectToAction("EmailConfiguration");
                }

                return View();
            }

            var serviceEndpointResult = await _emailService.GetEndpointOfEmailService();

            if (serviceEndpointResult.Succeeded)
            {
                ViewBag.ServiceEndpoints = serviceEndpointResult.Data;
            }
            else
            {
                ViewBag.ServiceEndpoints = Enumerable.Empty<ServiceEndpointResponse>();
                ViewBag.ErrorMessage = "Failed to fetch service endpoints.";
            }

            return View();
        }


        [Authorize]
        [HttpPost]

        public async Task<IActionResult> EmailConfiguration(EmailConfigurationViewModel emailConfigurationViewModel, List<long> EndpointsId)
        {
            if (!ModelState.IsValid)
                return View(emailConfigurationViewModel);

            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            emailConfigurationViewModel.UserId = Int32.Parse(userId);
            emailConfigurationViewModel.ServiceId = 1;

            var createEmailConfigResult = await _emailService.CreateEmailConfiguration(emailConfigurationViewModel);
            if (createEmailConfigResult.Succeeded)
            {
                if (EndpointsId.Any())
                {
                    var createUserApiKeyRequest = new CreateUserApiKeyRequest
                    {
                        ServiceId = 1,
                        UserId = Int32.Parse(userId)
                    };

                    var createUserApiKeyResult = await _keyService.CreateUserApiKey(createUserApiKeyRequest);

                    if (createUserApiKeyResult.Succeeded)
                    {
                        var createKeyAllowedEndpoint = new CreateKeyAllowedEndpointRequest
                        {
                            EndpointId = EndpointsId,
                            UserApiKeyId = createUserApiKeyResult.Data
                        };

                        var createKeyAllowedEndpointResult = await _keyService.CreateKeyAllowedEndpoint(createKeyAllowedEndpoint);

                        if (createKeyAllowedEndpointResult.Succeeded)
                        {
                            return RedirectToAction("Email");
                        }
                    }

                    return RedirectToAction("Privacy", "Home");
                }
                else
                {
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false, message = createEmailConfigResult.ErrorMessage });


        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserEmailConfig(UpdateUserEmailConfigRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _emailService.UpdateEmailConfiguration(request);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Config updated successfully.";
                }
                else
                {
                    TempData["Error"] = result.ErrorMessage;
                }
            }
            return RedirectToAction("Email");
        }
        [HttpGet]
        public async Task<IActionResult> GetConfigUpdateModal(long id)
        {
            // Fetch the endpoint data from the service or database
            var endpoint = await _emailService.GetEmailConfigurationById(id);
            if (endpoint == null)
            {
                return NotFound("Endpoint not found.");
            }

            // Return the partial view with the model
            return PartialView("_UpdateUserEmailConfigPartial", endpoint.Data);
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ApiKeyEmail()
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var userKeys = await _keyService.GetKeyDetail(Int32.Parse(userId), 1);

            ViewBag.UserKeys = userKeys.Data.OrderByDescending(k => k.CreatedOn).ToList();

            var userKeysActive = userKeys.Data.Where(k => k.IsActive).ToList();

            if (userKeysActive.Any())
            {
                ViewBag.UserKeyActive = true;
            }

            var serviceEndpointResult = await _emailService.GetEndpointOfEmailService();

            if (serviceEndpointResult.Succeeded)
            {
                ViewBag.ServiceEndpoints = serviceEndpointResult.Data;
            }
            else
            {
                ViewBag.ServiceEndpoints = Enumerable.Empty<ServiceEndpointResponse>();
                ViewBag.ErrorMessage = "Failed to fetch service endpoints.";
            }

            return View();
        }


        [Authorize]
        [HttpPost]

        public async Task<IActionResult> ApiKeyEmail(List<long> EndpointsId)
        {


            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var createUserApiKeyRequest = new CreateUserApiKeyRequest
            {
                ServiceId = 1,
                UserId = Int32.Parse(userId)
            };

            var createUserApiKeyResult = await _keyService.CreateUserApiKey(createUserApiKeyRequest);

            if (createUserApiKeyResult.Succeeded)
            {
                var createKeyAllowedEndpoint = new CreateKeyAllowedEndpointRequest
                {
                    EndpointId = EndpointsId,
                    UserApiKeyId = createUserApiKeyResult.Data
                };

                var createKeyAllowedEndpointResult = await _keyService.CreateKeyAllowedEndpoint(createKeyAllowedEndpoint);

                if (createKeyAllowedEndpointResult.Succeeded)
                {
                    return RedirectToAction("EmailConfiguration");
                }
                return RedirectToAction("Privacy", "Home");
            }
            else
            {
                // Handle login failure
                TempData["ErrorMessage"] = createUserApiKeyResult.ErrorMessage;
                return View(EndpointsId);
            }

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EmailConfiguration()
        {

            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var userKeys = await _keyService.GetKeyDetail(Int32.Parse(userId), 1);

            var userEmailConfigs = await _emailService.GetEmailConfigurationDetailByUserId(Int64.Parse(userId));

            ViewBag.EmailConfigurations = userEmailConfigs.Data.ToList();

            ViewBag.UserKeys = userKeys.Data.OrderByDescending(k => k.CreatedOn).ToList();

            var userKeysActive = userKeys.Data.Where(k => k.IsActive).ToList();

            if (userKeysActive.Any())
            {
                ViewBag.UserKeyActive = true;
            }

            var serviceEndpointResult = await _emailService.GetEndpointOfEmailService();

            if (serviceEndpointResult.Succeeded)
            {
                ViewBag.ServiceEndpoints = serviceEndpointResult.Data;
            }
            else
            {
                ViewBag.ServiceEndpoints = Enumerable.Empty<ServiceEndpointResponse>();
                ViewBag.ErrorMessage = "Failed to fetch service endpoints.";
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateApiKeyStatus(UpdateUserApiKeyRequest updateUserApiKeyRequest)
        {
            if (updateUserApiKeyRequest == null || updateUserApiKeyRequest.Id <= 0)
            {
                return BadRequest("Invalid request.");
            }

            var updateUserApiKeyResult = await _keyService.UpdateUserApiKeyStatus(updateUserApiKeyRequest);
            if (updateUserApiKeyResult.Succeeded)
            {
                TempData["SuccessMessage"] = "API key deleted successfully.";
                return RedirectToAction("EmailConfiguration"); // Replace with your desired action
            }

            TempData["ErrorMessage"] = "Failed to delete the API key.";
            return RedirectToAction("EmailConfiguration");
        }

    }
}
