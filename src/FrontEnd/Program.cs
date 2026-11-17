using FrontEnd;
using FrontEnd.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var backEndUrl = builder.Configuration["BackEndUrl"] ?? "http://localhost:5100";
builder.Services.AddHttpClient<WeatherForecastClient>(client => client.BaseAddress = new Uri(backEndUrl));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
