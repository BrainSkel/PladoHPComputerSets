using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PladoHPComputerSets.Data;
using PladoHPComputerSets.Models;
using PladoHPComputerSets;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole",
         policy => policy.RequireRole("Admin"));
});


var app = builder.Build();

var serviceScope = app.Services.CreateScope();
var RoleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var UserManager = serviceScope.ServiceProvider.GetService<UserManager<IdentityUser>>();

string[] roleNames = { "Admin", "Manager", "Member" };
IdentityResult roleResult;

foreach (var roleName in roleNames)
{
    var roleExist = await RoleManager.RoleExistsAsync(roleName);
    if (!roleExist)
    {
        //create the roles and seed them to the database: Question 1
        roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
    }
}

//Here you could create a super user who will maintain the web app
var poweruser = new IdentityUser
{
    UserName = builder.Configuration["UserName"],
    Email = builder.Configuration["UserEmail"],
};
//Ensure you have these values in your appsettings.json file
string userPWD = builder.Configuration["UserPassword"];
var _user = await UserManager.FindByEmailAsync(builder.Configuration["AdminUserEmail"]);

if (_user == null)
{
    var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
    if (createPowerUser.Succeeded)
    {
        //here we tie the new user to the role
        await UserManager.AddToRoleAsync(poweruser, "Admin");

    }
}



if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();