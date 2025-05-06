using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Library.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<User_Employee_Service>();

// 🔧 Registriere UserRepository für Dependency Injection
builder.Services.AddScoped<UserRepository>();

var app = builder.Build();

Database.Initialize();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseHttpsRedirection(); // Nur in Produktion nötig
}

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
