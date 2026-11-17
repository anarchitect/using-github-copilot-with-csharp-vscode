namespace FrontEnd;

public class WeatherForecastClient(HttpClient httpClient)
{
    public async Task<WeatherForecast[]> GetForecastAsync() =>
        await httpClient.GetFromJsonAsync<WeatherForecast[]>("weatherforecast") ?? [];
}

public record WeatherForecast(string City, DateOnly Date, int TemperatureC, string? Summary, int TemperatureF);
