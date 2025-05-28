// File: Program.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServerApi.Interface;
using ServerApi.Repository;

var builder = WebApplication.CreateBuilder(args);

// ──────────────────────────────────────────────────────────────────────────────
// 1) MVC / Controllers, Session, Caching, CORS
// ──────────────────────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddCors(opts =>
{
    opts.AddPolicy("AllowAll", b =>
        b.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// ──────────────────────────────────────────────────────────────────────────────
// 2) Registrér dine repository‐implementeringer
//    Repositorierne tager selv connection‐string og åbner Mongo‐forbindelsen.
// ──────────────────────────────────────────────────────────────────────────────
builder.Services.AddSingleton<IAppUser,       AppUserRepository>();
builder.Services.AddSingleton<IElevplan,      ElevplanRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors("AllowAll");
app.UseSession();

app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.Run();