using SSO.Models;
using SSO;
using Microsoft.Extensions.Configuration;
using SSO.Data;
using Microsoft.EntityFrameworkCore;
using SSO.Controllers;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContextPool<DbContext, ApplicationDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<ApplicationUser>(opt =>
{
    opt.User.RequireUniqueEmail = false;
    opt.Lockout.AllowedForNewUsers = true; // Default true
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // Default 5
    opt.Lockout.MaxFailedAccessAttempts = 3; // Default 5
})
    .AddSignInManager<SignInManager<ApplicationUser>>()
           .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.AllowedForNewUsers = true; // Default true
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // Default 5
    options.Lockout.MaxFailedAccessAttempts = 3; // Default 5
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
    options.Lockout.AllowedForNewUsers = true;
});

builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;

    // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
    //options.EmitStaticAudienceClaim = true;
})
     //.AddTestUsers(TestUsers.Users)
        .AddInMemoryIdentityResources(Config.IdentityResources)
        .AddInMemoryApiScopes(Config.ApiScopes)
        .AddInMemoryClients(Config.Clients)
        //.AddAspNetIdentity<ApplicationUser>()
        .AddDeveloperSigningCredential();
builder.Services.AddAuthentication();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
