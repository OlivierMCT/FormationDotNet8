using Microsoft.AspNetCore.Mvc.Filters;

namespace CATodos.Api.Filters {
    public class ResponseHeaderResultFilterAttribute : ResultFilterAttribute {
        public List<(string key, string value)>? Headers { get; init; }

        public override void OnResultExecuting(ResultExecutingContext context) {
            Headers?.ForEach(x => context.HttpContext.Response.Headers.Append(x.key, x.value));
        }
    }
}
