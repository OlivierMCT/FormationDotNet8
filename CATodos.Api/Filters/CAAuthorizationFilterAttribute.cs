using CATodos.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CATodos.Api.Filters {
    public class CAAuthorizationFilterAttribute : Attribute, IAuthorizationFilter {
        private string _applications = string.Empty;
        public string Applications {
            get { return _applications; }
            set { 
                _applications = value;
                _applicationList = _applications.Split(";").ToList();
            }
        }
        private List<string>? _applicationList = null;


        public void OnAuthorization(AuthorizationFilterContext context) {
            if(context.HttpContext.Items.TryGetValue("CAApplication", out object? obj) && obj is CAAuthenticationApplication app) { 
                if(_applicationList != null && !_applicationList.Any(n => n == app.Name)) {
                    context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                }
            } else {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
