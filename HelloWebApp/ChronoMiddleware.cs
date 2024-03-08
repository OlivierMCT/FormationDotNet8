using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HelloWebApp {
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ChronoMiddleware {
        private readonly RequestDelegate _next;

        public ChronoMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ILogger<ChronoMiddleware> logger, IConfiguration conf) {
            var chrono = Stopwatch.StartNew();
            await _next(httpContext);
            chrono.Stop();

            string log = $"Temps d'exécution = {chrono.ElapsedMilliseconds}";
            int sinfo = conf.GetValue<int>("Chrono:Seuils:Info");

            var sectionChrono = conf.GetRequiredSection("Chrono");
            int sdanger = sectionChrono.GetValue<int>("Seuils:Danger");

            if (chrono.ElapsedMilliseconds <= sinfo)
                logger.LogInformation(log);
            else if (chrono.ElapsedMilliseconds <= sdanger)
                logger.LogWarning(log);
            else {
                logger.LogError(log);
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ChronoMiddlewareExtensions {
        public static IApplicationBuilder UseChronoMiddleware(this IApplicationBuilder builder) {
            return builder.UseMiddleware<ChronoMiddleware>();
        }
    }
}
