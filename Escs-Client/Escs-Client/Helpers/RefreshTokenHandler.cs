using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Escs_Client.Helpers
{
    public class RefreshTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public RefreshTokenHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AccessToken")?.Value;

            // Attach the token to the Authorization header
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var response = await base.SendAsync(request, cancellationToken);

            // If token is expired, refresh and retry
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (await RefreshAccessTokenAsync())
                {
                    // Retry the original request with the new token
                    accessToken = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AccessToken")?.Value;
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        response = await base.SendAsync(request, cancellationToken);
                    }
                }
                else
                {
                    await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }
            }

            return response;
        }

        private async Task<bool> RefreshAccessTokenAsync()
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return false;

            var httpClient = new HttpClient();
            var refreshEndpoint = _configuration["ApiSettings:RefreshTokenEndpoint"];

            var response = await httpClient.PostAsJsonAsync(refreshEndpoint, new
            {
                RefreshToken = refreshToken
            });

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<RefreshTokenResponse>();

            if (result == null || string.IsNullOrEmpty(result.AccessToken))
                return false;

            // Update the stored tokens
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(result.AccessToken);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value ?? string.Empty),
            new Claim(ClaimTypes.Email, jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value ?? string.Empty),
            new Claim(ClaimTypes.Name, jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value ?? string.Empty),
            new Claim("AccessToken", result.AccessToken) // Store the access token in a claim
        };

            // Add role claims if present
            foreach (var roleClaim in jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role))
            {
                claims.Add(new Claim(ClaimTypes.Role, roleClaim.Value));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));


            _httpContextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions { HttpOnly = true, Secure = true });
            return true;
        }
    }

    public class RefreshTokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

}
