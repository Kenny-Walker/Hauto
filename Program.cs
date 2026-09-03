using Hauto;
using Hauto.Context;
using Hauto.Implementations.Repository;
using Hauto.Implementations.Service;
using Hauto.Interface.IRepository;
using Hauto.Interface.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(x => x.AddPolicy("Policies", c =>
{
    c.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
}));
// Add services to the container.
builder.Services.AddScoped<IDeviceRepo, DeviceRepo>();
builder.Services.AddScoped<ILogRepo, LogRepo>();
builder.Services.AddScoped<IScheduleRepo, ScheduleRepo>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddHttpContextAccessor();
var connectionString = builder.Configuration.GetConnectionString("Hauto");
builder.Services.AddDbContext<HautoContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo { Title = "Hauto", Version = "v1" });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:7282","https://127.0.0.1:5254")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddRateLimiter(options =>
{
    // 1. Set a higher fixed window limit (e.g., 1000 requests per minute instead of a low default)
    options.AddFixedWindowLimiter("HautoPolicy", opt =>
    {
        opt.PermitLimit = 100;              // INCREASE THIS NUMBER to allow more requests
        opt.Window = TimeSpan.FromMinutes(1); // Time window duration
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 50;                 // How many requests can wait in line if limit is hit
    });

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsync("Rate limit exceeded. Please try again later.");
    };
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

app.MapControllers();

app.Run();
