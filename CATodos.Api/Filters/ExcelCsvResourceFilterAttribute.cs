using Microsoft.AspNetCore.Mvc.Filters;

namespace CATodos.Api.Filters {
    public class ExcelCsvResourceFilterAttribute : Attribute, IResourceFilter {
        public void OnResourceExecuting(ResourceExecutingContext context) {
            if (context.HttpContext.Request.Headers["User-Agent"].ToString().Contains("Microsoft", StringComparison.InvariantCultureIgnoreCase)) {
                context.HttpContext.Request.Headers["Accept"] = "text/csv";
                context.HttpContext.RequestServices
                    .GetService<ILogger<ExcelCsvResourceFilterAttribute>>()
                    ?.LogInformation("{} requested by Excel => Accept = text/csv", context.ActionDescriptor.DisplayName);
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context) {}
    }
}
