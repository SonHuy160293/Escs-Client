using Escs_Client.Models;
using System.Diagnostics;

namespace Escs_Client.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception)
            {

                // Store error details in TempData for MVC Error controller
                context.Items["Error"] = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? context.TraceIdentifier
                };

                // Redirect to the Error action of the HomeController
                context.Response.Redirect("/Home/Error");
            }
        }

    }

}
