using BaseManager;
using CoreApp;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddSingleton<TransaccionManager>();
builder.Services.AddSingleton<ComisionManager>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy
            .WithOrigins("https://localhost:7060")  // Or "*" para pruebas, pero no recomendado en producción
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Error/{0}");
//app.MapFallbackToPage("/Welcome");

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
