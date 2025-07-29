using JobPortalReal.DataAccess.Data;
using JobPortalReal.Services.Implementations;
using JobPortalReal.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure DbContext
builder.Services.AddDbContext<JobPortalContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<INotificationService, NotificationService>();

// Add CORS services
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


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Use CORS policy
app.UseCors("AllowLocalhost");

app.MapControllers();

app.Run();
