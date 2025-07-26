using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5221", "https://localhost:5001");

// 2) Tus servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login"; 
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
var app = builder.Build();

app.UseCors("AllowLocalhost");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 3) ¡Importante!  
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// 5) Luego mapea los controladores
app.MapControllers();

app.Run();