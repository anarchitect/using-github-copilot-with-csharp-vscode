using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BackEnd.Tests;

public class WeatherForecastEndpointTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetWeatherForecast_ReturnsFiveDaysForEachCity()
    {
        var client = factory.CreateClient();

        var forecasts = await client.GetFromJsonAsync<WeatherForecast[]>("/weatherforecast");

        Assert.NotNull(forecasts);
        Assert.Equal(15, forecasts.Length);
        Assert.All(forecasts.GroupBy(f => f.City), group => Assert.Equal(5, group.Count()));
    }

    [Fact]
    public async Task GetWeatherForecastByCity_IsCaseInsensitive()
    {
        var client = factory.CreateClient();

        var forecasts = await client.GetFromJsonAsync<WeatherForecast[]>("/weatherforecast/sydney");

        Assert.NotNull(forecasts);
        Assert.Equal(5, forecasts.Length);
        Assert.All(forecasts, f => Assert.Equal("Sydney", f.City));
    }

    [Fact]
    public async Task GetWeatherForecastByCity_UnknownCity_ReturnsNotFound()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/weatherforecast/Atlantis");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task OpenApiDocument_IsAvailable()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/openapi/v1.json");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public void TemperatureF_IsCalculatedFromCelsius()
    {
        var forecast = new WeatherForecast("Brisbane", DateOnly.FromDateTime(DateTime.Today), 0, "Cool");

        Assert.Equal(32, forecast.TemperatureF);
    }
}
