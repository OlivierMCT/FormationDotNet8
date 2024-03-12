using Microsoft.Extensions.Options;

namespace CATodos.Api.Services {
    public interface ICAAuthenticationService {
        CAAuthenticationApplication? GetApplicationByAccessKey(string accessKey);
    }

    public class CAAuthenticationService(ICAAuthenticationProviderService provider) : ICAAuthenticationService {
        public CAAuthenticationApplication? GetApplicationByAccessKey(string accessKey) { 
            return provider.Applications.FirstOrDefault(app => app.Key == accessKey);
        }
    }


    public interface ICAAuthenticationProviderService {
        List<CAAuthenticationApplication> Applications { get; }
    }

    public class CAAuthenticationInSettingsProviderService : ICAAuthenticationProviderService {
        public List<CAAuthenticationApplication> Applications { get; }

        public CAAuthenticationInSettingsProviderService(
            IOptions<List<CAAuthenticationApplication>> applications,
            ILogger<CAAuthenticationInSettingsProviderService> logger
        ) {
            Applications = applications.Value;
            string log = $"CAAuthentication Applications List\n{"Application", -30}| Key\n{"".PadLeft(72, '-')}\n";
            Applications.ForEach(app => log += $"{app.Name,-30}| {app.Key,-40}\n");
            logger.LogInformation(log);
        }
    }

    public class CAAuthenticationApplication {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public string Key { get; init; } = null!;
    }

    public static class CAAuthenticationExtensions { 
        public static IHostApplicationBuilder AddCAAuthentication(this IHostApplicationBuilder builder) {
            builder.Services
                .AddSingleton<ICAAuthenticationService, CAAuthenticationService>()
                .AddSingleton<ICAAuthenticationProviderService, CAAuthenticationInSettingsProviderService>()
                .Configure<List<CAAuthenticationApplication>>(
                    builder.Configuration.GetSection("CAAuthentication:Applications")
                );
            return builder;
        }
    }
}
