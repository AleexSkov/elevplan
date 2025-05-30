// File: Program.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServerApi.Interface;
using ServerApi.Repository;

var builder = WebApplication.CreateBuilder(args);

// ──────────────────────────────────────────────────────────────────────────────
// 1) Tilføj services til DI-containeren
// ──────────────────────────────────────────────────────────────────────────────

// Tilføjer controller-understøttelse (API-endpoints)
builder.Services.AddControllers();

// Gør det muligt at få adgang til HttpContext via dependency injection
builder.Services.AddHttpContextAccessor();

// Tilføjer in-memory cache (påkrævet af session)
builder.Services.AddDistributedMemoryCache();

// Tilføjer session-support (bruges til loginstatus m.m.)
builder.Services.AddSession();

// Konfigurer CORS-politik (tillad alle oprindelser – typisk til lokal udvikling)
builder.Services.AddCors(opts =>
{
    opts.AddPolicy("AllowAll", b =>
        b.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader());
});

// ──────────────────────────────────────────────────────────────────────────────
// 2) Registrering af repositories i Dependency Injection (DI)
// ──────────────────────────────────────────────────────────────────────────────

// Repositories implementerer interfaces og håndterer dataadgang
builder.Services.AddSingleton<IAppUser,  AppUserRepository>();
builder.Services.AddSingleton<IElevplan, ElevplanRepository>();

// ──────────────────────────────────────────────────────────────────────────────
// 3) Byg og konfigurer middleware-pipelinen
// ──────────────────────────────────────────────────────────────────────────────
var app = builder.Build();

// Vis detaljeret fejlside i udviklingsmiljø
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Aktiver CORS-politik
app.UseCors("AllowAll");

// Aktiver sessionhåndtering
app.UseSession();

// Konfigurer routing (kræves af endpoints)
app.UseRouting();

// Kortlæg API-endpoints til controllere
app.UseEndpoints(endpoints => endpoints.MapControllers());

// Sørg for, at Blazor-filer og statiske filer bliver håndteret korrekt
app.UseBlazorFrameworkFiles(); // Bruger Blazor WebAssembly-filer
app.UseStaticFiles();          // Håndterer fx wwwroot-filer

// Hvis ingen rute matcher, falder vi tilbage til index.html (SPA-routing)
app.MapFallbackToFile("index.html");

// Start appen
app.Run();
