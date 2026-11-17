using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;

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
        Assert.Equal(["Brisbane", "Melbourne", "Sydney"], forecasts.Select(f => f.City).Distinct().Order());
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
    public async Task GetCities_ReturnsTheThreeSampleCities()
    {
        var client = factory.CreateClient();

        var cities = await client.GetFromJsonAsync<string[]>("/weatherforecast/cities");

        Assert.NotNull(cities);
        Assert.Equal(["Brisbane", "Melbourne", "Sydney"], cities.Order());
    }

    [Fact]
    public async Task ScalarReference_IsAvailable()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/scalar");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("text/html", response.Content.Headers.ContentType?.ToString());
    }

    [Fact]
    public async Task OpenApiDocument_DescribesEveryWeatherOperation()
    {
        var client = factory.CreateClient();
        using var document = JsonDocument.Parse(await client.GetStringAsync("/openapi/v1.json"));
        var paths = document.RootElement.GetProperty("paths");

        foreach (var path in new[] { "/weatherforecast", "/weatherforecast/cities", "/weatherforecast/{city}" })
        {
            var operation = paths.GetProperty(path).GetProperty("get");
            Assert.False(string.IsNullOrWhiteSpace(operation.GetProperty("operationId").GetString()));
            Assert.False(string.IsNullOrWhiteSpace(operation.GetProperty("summary").GetString()));
            Assert.False(string.IsNullOrWhiteSpace(operation.GetProperty("description").GetString()));
        }
    }

    [Theory]
    [InlineData("/openapi/v1.json")]
    [InlineData("/scalar")]
    public async Task ApiDocumentation_IsNotExposedInProduction(string path)
    {
        using var production = factory.WithWebHostBuilder(builder => builder.UseEnvironment("Production"));
        var client = production.CreateClient();

        var response = await client.GetAsync(path);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData(-40, -40)]
    [InlineData(-20, -4)]
    [InlineData(0, 32)]
    [InlineData(1, 34)]
    [InlineData(37, 99)]
    [InlineData(100, 212)]
    public void TemperatureF_IsRoundedToTheNearestDegree(int celsius, int fahrenheit)
    {
        var forecast = new WeatherForecast("Brisbane", DateOnly.FromDateTime(DateTime.Today), celsius, "Cool");

        Assert.Equal(fahrenheit, forecast.TemperatureF);
    }
}
