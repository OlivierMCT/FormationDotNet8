using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace CATodos.Api.Filters {
    public class ChronoActionFilterAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext context) {
            var chrono = Stopwatch.StartNew();
            context.HttpContext.Items.Add("ChronoActionFilter:Chrono", chrono);
        }

        public override void OnActionExecuted(ActionExecutedContext context) {
            if (context.HttpContext.Items.TryGetValue("ChronoActionFilter:Chrono", out object? data) && data is Stopwatch chrono) {
                chrono.Stop();
                context.HttpContext
                    .RequestServices
                    .GetService<ILogger<ChronoActionFilterAttribute>>()
                    ?.LogInformation("{}ms to execute {}", chrono.ElapsedMilliseconds, context.ActionDescriptor.DisplayName);
            }
        }
    }
}
