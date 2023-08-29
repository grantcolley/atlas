using Atlas.API.Endpoints;
using Atlas.Core.Models;
using Atlas.Data.Access.Data;
using Atlas.Data.Access.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Auth0:Domain"],
            ValidAudience = builder.Configuration["Auth0:Audience"]
        };
    });

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("atlas-user", p => p.
        RequireAuthenticatedUser().
        RequireClaim("role", "atlas-user"));
});

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

app.UseAuthentication();

app.UseAuthorization();

app.MapHealthChecks("/health");

app.MapGet("/error", () => Results.Problem());

app.MapGet("/weatherforecast", WeatherForecastEndpoint.GetWeatherForecast)
.WithName("GetWeatherForecast")
.WithOpenApi()
.WithName("GetBestStories")
.WithDescription("The GetBestStories Endpoint")
.Produces<IEnumerable<WeatherForecast>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status500InternalServerError)
.RequireAuthorization("atlas-user");

app.Run();