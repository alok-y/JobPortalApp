using JobPortalReal.DataAccess.Data;
using JobPortalReal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JobPortalReal.Services.Interfaces;
using JobPortalReal.Services.Implementations;
using Microsoft.Extensions.Caching.Distributed;



var builder = WebApplication.CreateBuilder(args);

//adds support for controllers with views to the service container, enabling MVC pattern
builder.Services.AddControllersWithViews();



builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Auto-logout after 30 mins
    options.SlidingExpiration = false;
    options.Cookie.IsEssential = true; // Ensure essential authentication
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    // Remove options.Cookie.Expiration
});


//Configures(SetsUp) the JobPortalContext to use SQL Server with a connection string from the configuration file.
builder.Services.AddDbContext<JobPortalContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlOptions => sqlOptions.CommandTimeout(180)));




// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<JobPortalContext>()
    .AddDefaultTokenProviders();

//registering Iuserservice, ijobservice,INotificationService
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<INotificationService, NotificationService>();


// Add CORS services
//CORS is a way to control who can access resources on your server from different websites
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder =>
        {
            builder.WithOrigins("https://localhost:7149", "https://localhost:7238")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});


//Take all the configurations I've done so far and create the web application.
var app = builder.Build();

// Seed database on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedAsync(services);
}

// Configure the HTTP request pipeline
//HSTS stands for HTTP Strict Transport Security.
//It tells browsers to only communicate with the server over HTTPS
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
// Use CORS policy
app.UseCors("AllowLocalhost");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();




app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
