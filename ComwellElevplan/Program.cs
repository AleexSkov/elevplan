using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ComwellElevplan;
using Core.Services;
using Core.Interface;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }); 

builder.Services.AddScoped(sp =>
{
    var baseUri = builder.HostEnvironment.IsDevelopment()
        ? "https://localhost:5102/" // ← din API-port lokalt
        : builder.HostEnvironment.BaseAddress; // ← på Azure

    return new HttpClient { BaseAddress = new Uri(baseUri) };
});

// ✅ Use mock repository for development
builder.Services.AddScoped<IElevplan, MockElevplanRepository>();

// ✅ ElevplanService works because IElevplan is registered
builder.Services.AddScoped<IElevplanService, ElevplanService>();

await builder.Build().RunAsync();