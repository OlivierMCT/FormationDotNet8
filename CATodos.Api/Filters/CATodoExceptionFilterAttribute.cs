using CATodos.BusinessModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CATodos.Api.Filters {
    public class CATodoExceptionFilterAttribute : ExceptionFilterAttribute {
        public override void OnException(ExceptionContext context) {
            if(context.Exception is CATodoException ex) {
                context.Result = ex.Code switch { 
                    101 or 102 => new NotFoundObjectResult(ex.Message),
                    103 or 104 => new BadRequestObjectResult(ex.Message),
                    _ => null
                };
                context.ExceptionHandled = context.Result != null;
            }
        }
    }
}
