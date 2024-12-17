using BuildingBlocks.Common;
using BuildingBlocks.Interfaces;
using Escs_Client.Interfaces;
using Escs_Client.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Escs_Client.Services
{
    public class AuthenService : IAuthenService
    {

        private readonly IHttpCaller _httpCaller;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthenService> _logger;
        public AuthenService(IHttpCaller httpCaller, IHttpContextAccessor httpContextAccessor, ILogger<AuthenService> logger)
        {
            _httpCaller = httpCaller;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<BaseResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                // Call the Login API to get the accessToken and refreshToken
                var httpCallOptions = HttpCallOptions<LoginViewModel>.UnAuthenticated(
                    isSerialized: true,
                    isRetry: false,
                    body: loginViewModel,
                    baseUrl: "http://localhost:5212/api/Authentications/login",
                    queryStringElements: new List<string>()
                );

                // Use LoginResponse as the data type
                var loginResult = await _httpCaller.PostAsync<LoginViewModel, LoginResponse>(httpCallOptions);



                if (loginResult?.Succeeded == true && loginResult.Data != null)
                {
                    // Access the tokens from the LoginResponse
                    var accessToken = loginResult.Data.AccessToken;
                    var refreshToken = loginResult.Data.RefreshToken;

                    // Decode the accessToken to extract claims
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(accessToken);

                    var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value ?? string.Empty),
            new Claim(ClaimTypes.Email, jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value ?? string.Empty),
            new Claim(ClaimTypes.Name, jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value ?? string.Empty),
            new Claim("AccessToken", accessToken) // Store the access token in a claim
        };

                    // Add role claims if present
                    foreach (var roleClaim in jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roleClaim.Value));
                    }

                    // Create a ClaimsIdentity and sign in the user
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    // Optionally, store the refreshToken in a secure cookie or session
                    _httpContextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions { HttpOnly = true, Secure = true });

                    return BaseResult.Success();

                }
                else
                {
                    return BaseResult.Failure(loginResult.StatusCode.ToString(), loginResult.ExceptionError.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "Log in occured exception");
                throw;
            }
        }

        public async Task<BaseResult> Logout()
        {
            try
            {
                // Retrieve the refresh token from cookies
                var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["RefreshToken"];
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return BaseResult.Failure("400", "Refresh token not found.");
                }
                var logoutRequest = new LogoutRequest()
                {
                    RefreshToken = refreshToken,
                };

                // Create options for HTTP call
                var httpCallOptions = HttpCallOptions<LogoutRequest>.UnAuthenticated(
                    isSerialized: true,
                    isRetry: false,
                    body: logoutRequest,
                    baseUrl: "http://localhost:5212/api/Authentications/logout",
                    queryStringElements: new List<string>());

                // Send the logout request to the external API
                var logoutResponse = await _httpCaller.PostAsync<LogoutRequest, bool>(httpCallOptions);

                // If the logout request is successful
                if (logoutResponse != null && logoutResponse.Succeeded)
                {
                    // Remove the refresh token from cookies
                    _httpContextAccessor.HttpContext.Response.Cookies.Delete("RefreshToken");

                    // Sign out the user
                    await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    return BaseResult.Success();
                }
                else
                {
                    // Return failure result if the API call was unsuccessful
                    return BaseResult.Failure(logoutResponse?.StatusCode.ToString() ?? "500", logoutResponse?.ExceptionError.Message ?? "Logout failed.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "Log out occured exception");
                throw;
            }
        }

    }
}
