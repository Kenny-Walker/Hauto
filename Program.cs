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
    c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));
// Add services to the container.
builder.Services.AddScoped<IDeviceRepo, DeviceRepo>();
builder.Services.AddScoped<ILogRepo, LogRepo>();
builder.Services.AddScoped<IScheduleRepo, ScheduleRepo>();
builder.Services.AddScoped<IHubsRepo, HubsRepo>();
builder.Services.AddScoped<IScheduledDeviceRepo, ScheduledDeviceRepo>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IHubsService, HubsService>();
builder.Services.AddSingleton<MqttService>();

builder.Services.AddSingleton<IMqttService>(
    provider => provider.GetRequiredService<MqttService>());

builder.Services.AddHostedService(
    provider => provider.GetRequiredService<MqttService>()); 
builder.Services.AddHostedService<MqttBackgroundRunService>();
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
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("HautoPolicy", opt =>
    {
        opt.PermitLimit = 100;               
        opt.Window = TimeSpan.FromMinutes(1); 
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 50;                
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

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
