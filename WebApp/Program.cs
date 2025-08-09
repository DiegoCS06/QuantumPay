using BaseManager;
using CoreApp;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using System;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddSingleton<TransaccionManager>();
builder.Services.AddSingleton<ComisionManager>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<WebAPI.Services.ComercioService>();
builder.Services.AddScoped<WebAPI.Services.TransaccionService>();
builder.Services.AddScoped<WebAPI.Services.CuentaComercioService>();

// Servicios para el panel de comercio:
builder.Services.AddHttpClient<TransaccionService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/");
});
builder.Services.AddHttpClient<CuentaComercioService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/");
});
builder.Services.AddHttpClient<ComercioService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/");
});

builder.Services.AddHttpClient<PromocionService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/");
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy
            .WithOrigins("https://localhost:7060")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/"); // Protege todo
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Welcome";
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Welcome";
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/AccessDenied";
    });
builder.Services.AddAuthorization();
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/"); // Protege todo
    options.Conventions.AllowAnonymousToPage("/Login");    // Excepción
    options.Conventions.AllowAnonymousToPage("/Welcome");  // Excepción
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", context =>
{
    context.Response.Redirect("/Welcome");
    return Task.CompletedTask;
});
app.MapRazorPages();
app.MapControllers();

app.Run();
