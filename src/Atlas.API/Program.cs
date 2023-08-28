using Atlas.API.Endpoints;
using Atlas.Core.Models;
using Atlas.Data.Access.Data;
using Atlas.Data.Access.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("local",
        builder =>
            builder.WithOrigins("https://localhost:44300", "https://localhost:44310")
                   .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.MapGet("/error", () => Results.Problem());

app.MapGet("/weatherforecast", WeatherForecastEndpoint.GetWeatherForecast)
.WithName("GetWeatherForecast")
.WithOpenApi()
.WithName("GetBestStories")
.WithDescription("The GetBestStories Endpoint")
.Produces<IEnumerable<WeatherForecast>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status500InternalServerError);

app.Run();