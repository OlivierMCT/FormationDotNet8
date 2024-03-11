using Microsoft.AspNetCore.Http.Extensions;

namespace HelloWebApp {
    public class TraceurMiddleware {
        private readonly RequestDelegate suivant;

        public TraceurMiddleware(RequestDelegate suivant) {
            this.suivant = suivant;
        }

        public Task Invoke(HttpContext http, IMagicService ms, IConfiguration config, ILogger<TraceurMiddleware> logger) {
            // config = variables env
            // + appstettings.json
            // + appsettings.{env}.json
            // + usersecrets.json
            // + add custom builder
            // + ligne de commande
            string protocole = http.Request.Scheme;
            string methode = http.Request.Method;
            string url = http.Request.GetDisplayUrl();
            logger.LogInformation($"--/{ms.MagicNumber}/-- {protocole} {methode} {url}");

            logger.LogInformation($"MyPATH : {config["MyPATH"]}");
            return suivant.Invoke(http);
        }
    }

    public static class TraceurMiddlewareExtensions {
        public static IApplicationBuilder UseTraceur(this IApplicationBuilder app) {
            return app.UseMiddleware<TraceurMiddleware>();
        }
    }
}
