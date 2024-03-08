namespace HelloWebApp {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
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
