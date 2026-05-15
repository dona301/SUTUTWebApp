using Microsoft.EntityFrameworkCore;
using SUTUTWebApp.Models.Entities;
using System;
using SUTUTWebApp.Repositories;
using SUTUTWebApp.Repositories.Interfaces;
using SUTUTWebApp.Services;
using SUTUTWebApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MasterContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IUtrkaRepository, UtrkaRepository>();
builder.Services.AddScoped<IStatusutrkeRepository, StatusutrkeRepository>();

// Services
builder.Services.AddScoped<IUtrkaService, UtrkaService>();
builder.Services.AddScoped<IStatusutrkeService, StatusutrkeService>();

var app = builder.Build();
var supportedCultures = new[] { System.Globalization.CultureInfo.InvariantCulture };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(
        System.Globalization.CultureInfo.InvariantCulture),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
