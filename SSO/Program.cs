using SSO.Models;
using SSO;
using Microsoft.Extensions.Configuration;
using SSO.Data;
using Microsoft.EntityFrameworkCore;
using SSO.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using SSO.Services;

var builder = WebApplication.CreateBuilder(args);
// Persistence Layer
//dotnet ef migrations add "Init" --project DoAn.Persistence --context ApplicationDbContext --startup-project DoAn.Api --output-dir Migrations 
// dotnet ef database update --project DoAn.Persistence  --startup-project DoAn.Api --context ApplicationDbContext   
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
builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie( options =>
{
    options.Cookie.Name = "idsrv";
    //options.Cookie.Path = "/";
    //options.ExpireTimeSpan = TimeSpan.FromDays(30);
    //options.Cookie.SameSite = SameSiteMode.None;
    //options.SlidingExpiration = true;
    ////options.Cookie.Domain = "lab.connect247.vn";

    //options.Cookie.IsEssential = true;
    //options.Cookie.HttpOnly = false;
    //options.Cookie.SecurePolicy = CookieSecurePolicy.None;
});


builder.Services.AddIdentityServer(options =>
    {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;
        options.Authentication.CookieSameSiteMode = SameSiteMode.None;

        // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
        options.EmitStaticAudienceClaim = true;
    })
    .AddDeveloperSigningCredential()
    //.AddTestUsers(TestUsers.Users)
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients)
    .AddProfileService<ProfileService>(); 
    // .AddAspNetIdentity<ApplicationUser>()
    ;



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
app.UseAuthorization();
app.UseIdentityServer();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
