using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using MiniArmory.Data.Data;
using MiniArmory.Data.Data.Models;

using MiniArmory.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAppDbContext(builder.Configuration);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<MiniArmoryDbContext>();

builder.Services
    .AddControllersWithViews(options =>
    {
        options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddMemoryCache();
builder.Services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "Area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
    name: "TwoPlayers",
    pattern: "{controller=Home}/{action=Index}/{id?}/{partnerId?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapRazorPages();
});

app.Run();