using CATodos.Api.Filters;
using CATodos.Api.Formatters;
using CATodos.Api.Middlewares;
using CATodos.Api.Services;
using CATodos.Business;
using CATodos.Persistance;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using WebApiContrib.Core.Formatter.Csv;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICATodoService, CATodoServiceDefaultImplementation>();
builder.Services.AddDbContext<CATodoDbContext>(
    optionBuilder => optionBuilder
        .EnableSensitiveDataLogging()
        .UseSqlServer(builder.Configuration.GetConnectionString("MyDb"))   
);

builder.Services.AddControllers(config => {
    config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
    config.OutputFormatters.Add(new CsvSerializerOutputFormatter());
    config.Filters.Add(new ResponseHeaderResultFilterAttribute() { Headers = [ ("Copyright", "CA-NMP"), ("Logo", "[(;-;)]") ] });
});

builder.AddCAAuthentication();
builder.Services.AddOutputCache(config => config.AddBasePolicy(p => p.Cache()));
builder.Services.AddSwaggerGen();

builder.Services.AddCors(config => {
    //config.AddPolicy("CA-NMP", p => p.WithOrigins("https://www.credit-agricole.fr").AllowAnyMethod().AllowAnyHeader());
    config.AddDefaultPolicy(p => p.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(s => true));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSampleDataMiddleware();
    app.UseSwagger().UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCAAuthentication();
app.UseOutputCache();
app.UseCors();
app.MapControllers();

app.Run();
