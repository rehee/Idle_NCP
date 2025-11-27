using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using IdleGame.Web;
using IdleGame.Core.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register game services for client-side use
builder.Services.AddSingleton<GameService>();
builder.Services.AddSingleton<GameStateService>(sp => new GameStateService(sp.GetRequiredService<GameService>()));

await builder.Build().RunAsync();
