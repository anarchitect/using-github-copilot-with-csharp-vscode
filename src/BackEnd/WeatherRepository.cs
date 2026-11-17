namespace BackEnd;

/// <summary>
/// Stores a generated 5-day weather forecast for a few cities in memory.
/// </summary>
public class WeatherRepository
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private static readonly string[] Cities = ["Brisbane", "Sydney", "Melbourne"];

    private readonly List<WeatherForecast> _forecasts;

    public WeatherRepository()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        _forecasts = Cities
            .SelectMany(city => Enumerable.Range(1, 5).Select(day => new WeatherForecast(
                city,
                today.AddDays(day),
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)])))
            .ToList();
    }

    /// <summary>Returns every forecast for every city.</summary>
    public Task<IReadOnlyList<WeatherForecast>> GetAllAsync() =>
        Task.FromResult<IReadOnlyList<WeatherForecast>>(_forecasts);

    /// <summary>Returns the forecasts for one city (case-insensitive).</summary>
    public Task<IReadOnlyList<WeatherForecast>> GetByCityAsync(string city) =>
        Task.FromResult<IReadOnlyList<WeatherForecast>>(
            _forecasts.Where(f => string.Equals(f.City, city, StringComparison.OrdinalIgnoreCase)).ToList());

    /// <summary>Returns the names of all cities that have forecasts.</summary>
    public Task<IReadOnlyList<string>> GetCitiesAsync() =>
        Task.FromResult<IReadOnlyList<string>>(_forecasts.Select(f => f.City).Distinct().ToList());
}
