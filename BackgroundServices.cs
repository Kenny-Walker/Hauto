using Hauto.Context;
using Hauto.Interface.IService;

namespace Hauto;

public class BackgroundServices : BackgroundService
{
    IServiceScopeFactory _serviceScopeFactory;
    IScheduleService scheduleService;
    public BackgroundServices(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    protected async override Task ExecuteAsync(CancellationToken token)
    {
        using var vscope = _serviceScopeFactory.CreateScope();
        var context = vscope.ServiceProvider.GetRequiredService<HautoContext>();
        await Task.CompletedTask;
    }
    protected async Task CheckScheduleAsync(int ScheduleId)
    {
        using var vscope = _serviceScopeFactory.CreateScope();
        var scheduleContext = vscope.ServiceProvider.GetRequiredService<IScheduleService>();
        await scheduleContext.CheckSchedule(ScheduleId);
        await Task.Delay(1800000);
        await Task.CompletedTask;
    }
}
public class MqttBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    public MqttBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var mqttService = scope.ServiceProvider.GetRequiredService<IMqttService>();
        await mqttService.ConnectAsync(stoppingToken);
    }
}

public class MqttBackgroundRunService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    public MqttBackgroundRunService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Configure the timer to tick every 1 minute
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

        // Wait for the next tick while respecting the application's stopping token
        while (!stoppingToken.IsCancellationRequested &&
               await timer.WaitForNextTickAsync(stoppingToken))
        {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<IDeviceService>();
                    //await dbContext.SyncDevices();
        }

    }
}