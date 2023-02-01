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

builder.Services.AddDefaultIdentity<IdentityUser>(options => { options.SignIn.RequireConfirmedAccount = true; })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole",
         policy => policy.RequireRole("Admin"));
});


var app = builder.Build();

using (var _serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var roleManager = _serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = _serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string[] roleNames = { "Admin", "Manager", "Member" };

    var userEmail = builder.Configuration.GetValue<string>("AdminUser:UserEmail");
    var userPassword = builder.Configuration.GetValue<string>("AdminUser:UserPassword");


    // Seed roles
    foreach (var roleName in roleNames)
    {
        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if (roleExists) continue;
        var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
        if (!roleResult.Succeeded) throw new Exception("Failed to create role: " + roleName);
    }

    // Create new admin user
    var userExists = await userManager.FindByEmailAsync(userEmail);
    if (userExists == null)
    {
        var user = new IdentityUser { UserName = userEmail, Email = userEmail, EmailConfirmed = true, LockoutEnabled = false };
        var userResult = await userManager.CreateAsync(user, userPassword);
        if (userResult.Succeeded)
        {
            // Assign role
            var roleResult = await userManager.AddToRoleAsync(user, "Admin");
            if (!roleResult.Succeeded) throw new Exception("Failed to assign role to user: " + userEmail);
        }
        else throw new Exception("Failed to create user: " + userEmail);
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