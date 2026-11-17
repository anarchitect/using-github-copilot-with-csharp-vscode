using BackEnd;
using Microsoft.AspNetCore.Http.HttpResults;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<WeatherRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // OpenAPI document at /openapi/v1.json and interactive API docs at /scalar
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/", () => "Weather API is running. Open /scalar for the API docs.")
    .ExcludeFromDescription();

var weather = app.MapGroup("/weatherforecast").WithTags("Weather");

weather.MapGet("/", async (WeatherRepository repo) => await repo.GetAllAsync())
    .WithName("GetWeatherForecast")
    .WithSummary("Get all forecasts")
    .WithDescription("Returns the 5-day forecast for every city.");

weather.MapGet("/cities", async (WeatherRepository repo) => await repo.GetCitiesAsync())
    .WithName("GetCities")
    .WithSummary("List cities")
    .WithDescription("Returns the names of all cities that have a forecast.");

weather.MapGet("/{city}", async Task<Results<Ok<IReadOnlyList<WeatherForecast>>, NotFound>> (string city, WeatherRepository repo) =>
    {
        var forecasts = await repo.GetByCityAsync(city);
        return forecasts.Count == 0 ? TypedResults.NotFound() : TypedResults.Ok(forecasts);
    })
    .WithName("GetWeatherForecastByCity")
    .WithSummary("Get the forecast for one city")
    .WithDescription("Returns the 5-day forecast for the given city, or 404 if the city is unknown.");

app.Run();

// Makes Program visible to the test project (WebApplicationFactory<Program>).
public partial class Program { }
