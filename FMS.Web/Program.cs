using Microsoft.AspNetCore.Authentication.Cookies;
using FMS.Data.Repository;
using FMS.Data.Services;
using FMS.Web;


var builder = WebApplication.CreateBuilder(args);
       
//Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options => {
             options.AccessDeniedPath = "/User/ErrorNotAuthorised";
             options.LoginPath = "/User/ErrorNotAuthenticated";
         });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    //The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
} else {
    //in development mode seed the database each time the application starts
    FleetServiceSeeder.Seed(new FleetServiceDb());
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ** Enable site Authentication/Authorization **
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
