using Hauto.Interface.IService;

namespace Hauto;

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