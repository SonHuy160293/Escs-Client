﻿using Escs_Client.Interfaces;
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


            var userId = Int32.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);


            if (!EndpointsId.Any() || EndpointsId is null && emailConfigurationViewModel is not null)
            {
                emailConfigurationViewModel.UserId = userId;
                emailConfigurationViewModel.ServiceId = 1;

                var createEmailConfigResult = await _emailService.CreateEmailConfiguration(emailConfigurationViewModel);

                if (createEmailConfigResult.Succeeded)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = createEmailConfigResult.ErrorMessage });
            }


            var createUserApiKeyAllowedEndpointTransactionRequest = new CreateUserApiKeyAllowedEndpointTransactionRequest
            {
                EndpointId = EndpointsId,
                ServiceId = 1,
                UserId = userId
            };

            var createUserApiKeyAllowedEndpointTransactionResponse = await _keyService.CreateUserApiKeyAllowedEndpointTransaction(createUserApiKeyAllowedEndpointTransactionRequest);
            if (createUserApiKeyAllowedEndpointTransactionResponse.Succeeded)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = createUserApiKeyAllowedEndpointTransactionResponse.ErrorMessage });


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

        [HttpGet]
        public async Task<IActionResult> GetKeyUpdateModal(long id)
        {
            var serviceEndpointResult = await _emailService.GetEndpointOfEmailService();

            // Fetch the endpoint data from the service or database
            var key = await _keyService.GetKeyDetailById(id);

            var endpointIds = key.Data.AllowedEndpoints.Select(a => a.EndpointId).ToList();

            if (key is null)
            {
                return NotFound("Endpoint not found.");
            }

            var endpointsNotChecked = serviceEndpointResult.Data.Where(e => !endpointIds.Contains(e.Id)).ToList();



            if (serviceEndpointResult.Succeeded)
            {
                ViewBag.EndpointsNotChecked = endpointsNotChecked;
            }
            else
            {
                ViewBag.EndpointsNotChecked = Enumerable.Empty<ServiceEndpointResponse>();
                ViewBag.ErrorMessage = "Failed to fetch service endpoints.";
            }
            // Return the partial view with the model
            return PartialView("_UpdateKeyPartial", key.Data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateKeyAllowed(UpdateEndpointOfKeyRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid request data." });
            }

            try
            {
                var result = await _keyService.UpdateUserApiKeyAllowed(request);

                if (result.Succeeded)
                {
                    return Json(new { success = true, message = "Key updated successfully." });
                }

                return Json(new { success = false, message = result.ErrorMessage });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "An unexpected error occurred." });
            }
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


        //[Authorize]
        //[HttpPost]

        //public async Task<IActionResult> ApiKeyEmail(List<long> EndpointsId)
        //{


        //    var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        //    var createUserApiKeyRequest = new CreateUserApiKeyRequest
        //    {
        //        ServiceId = 1,
        //        UserId = Int32.Parse(userId)
        //    };

        //    var createUserApiKeyResult = await _keyService.CreateUserApiKey(createUserApiKeyRequest);

        //    if (createUserApiKeyResult.Succeeded)
        //    {
        //        var createKeyAllowedEndpoint = new CreateKeyAllowedEndpointRequest
        //        {
        //            EndpointId = EndpointsId,
        //            UserApiKeyId = createUserApiKeyResult.Data
        //        };

        //        var createKeyAllowedEndpointResult = await _keyService.CreateKeyAllowedEndpoint(createKeyAllowedEndpoint);

        //        if (createKeyAllowedEndpointResult.Succeeded)
        //        {
        //            return RedirectToAction("EmailConfiguration");
        //        }
        //        return RedirectToAction("EmailConfiguration");
        //    }
        //    else
        //    {
        //        // Handle login failure
        //        TempData["ErrorMessage"] = createUserApiKeyResult.ErrorMessage;
        //        return View(EndpointsId);
        //    }

        //}

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
                return RedirectToAction("ApiKeyEmail"); // Replace with your desired action
            }

            TempData["ErrorMessage"] = "Failed to delete the API key.";
            return RedirectToAction("ApiKeyEmail");
        }

    }
}
