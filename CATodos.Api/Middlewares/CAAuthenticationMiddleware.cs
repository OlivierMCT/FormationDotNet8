using CATodos.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace CATodos.Api.Middlewares {
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CAAuthenticationMiddleware {
        private readonly RequestDelegate _next;

        public CAAuthenticationMiddleware(RequestDelegate next) {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext, ICAAuthenticationService authService, ILogger<CAAuthenticationMiddleware> logger) {
            if(httpContext.Request.Headers.TryGetValue("CAAccessKey", out StringValues value)) {
                CAAuthenticationApplication? app = authService.GetApplicationByAccessKey(value.ToString());
                if(app != null) {
                    httpContext.Items.Add("CAApplication", app);
                    logger.LogInformation("request made by {}", app.Name);
                }
            }

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CAAuthenticationMiddlewareExtensions {
        public static IApplicationBuilder UseCAAuthentication(this IApplicationBuilder builder) {
            return builder.UseMiddleware<CAAuthenticationMiddleware>();
        }
    }
}
