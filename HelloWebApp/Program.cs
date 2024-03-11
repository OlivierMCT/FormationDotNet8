namespace HelloWebApp {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            //builder.Services.AddSingleton<IMagicService, MagicService>();
            //builder.Services.AddTransient<IMagicService, MagicRangeService>();
            //builder.Services.AddTransient<IMagicService>(sp => new MagicCustomService(1048));

            builder.Services.AddKeyedSingleton(typeof(int), "MagicMin", 42);
            builder.Services.AddSingleton<IMagicService>(sp => {
                int min = sp.GetRequiredKeyedService<int>("MagicMin");
                IConfiguration conf = sp.GetRequiredService<IConfiguration>();
                return new MagicCustomService(min);
            });


            //builder.Services.AddScoped<IMagicService, MagicService>();

            //builder.Configuration.AddCommandLine(args);
            var app = builder.Build();

            app.UseChronoMiddleware();
            app.UseTraceur();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
