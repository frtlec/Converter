using Blazor.UI;
using Blazor.UI.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddSingleton<StateContainer>();
JsonConvert.DefaultSettings = (() =>
{
    var settings = new JsonSerializerSettings();
    settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
    return settings;
});
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 1000;
    config.SnackbarConfiguration.HideTransitionDuration = 600;
    config.SnackbarConfiguration.ShowTransitionDuration = 50;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
    config.SnackbarConfiguration.ClearAfterNavigation = true;
});
string baseAddress = builder.Configuration.GetValue<string>("BaseUrl");
Console.WriteLine(baseAddress);
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

builder.Services.AddScoped<FollowedProductService>();
builder.Services.AddScoped<TahtakaleService>();

await builder.Build().RunAsync();
