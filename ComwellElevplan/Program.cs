using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ComwellElevplan;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
    var baseUri = builder.HostEnvironment.IsDevelopment()
        ? "http://localhost:5102/" // ← din API-port lokalt
        : builder.HostEnvironment.BaseAddress; // ← på Azure
    return new HttpClient { BaseAddress = new Uri(baseUri) };
});

var host = builder.Build();
await host.RunAsync();