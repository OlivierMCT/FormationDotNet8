using CATodos.Api.Middlewares;
using CATodos.Business;
using CATodos.Persistance;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICATodoService, CATodoServiceDefaultImplementation>();
builder.Services.AddDbContext<CATodoDbContext>(
    optionBuilder => optionBuilder
        .EnableSensitiveDataLogging()
        .UseSqlServer(builder.Configuration.GetConnectionString("MyDb"))   
);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSampleDataMiddleware();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
