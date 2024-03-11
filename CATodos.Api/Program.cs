using CATodos.Api.Formatters;
using CATodos.Api.Middlewares;
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
    //config.OutputFormatters.Add(new CsvOutputFormatter(new CsvFormatterOptions() { Encoding = System.Text.Encoding.UTF8, IncludeExcelDelimiterHeader = true, CsvDelimiter = ";"}));
    config.OutputFormatters.Add(new CsvSerializerOutputFormatter());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSampleDataMiddleware();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
