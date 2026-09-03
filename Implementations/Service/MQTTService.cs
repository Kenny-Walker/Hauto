using System.Text;
using System.Text.Json;
using Hauto.Interface.IService;
using Hauto.Models.DTOs;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
namespace Hauto.Implementations.Service;
public class MqttService : BackgroundService, IMqttService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<MqttService> _logger;
    private IMqttClient _client;

    public MqttService(IConfiguration configuration, IServiceScopeFactory scopeFactory, ILogger<MqttService> logger)
    {
        _configuration = configuration;
        _scopeFactory = scopeFactory;
        _logger = logger;
        var factory = new MqttFactory();
        _client = factory.CreateMqttClient();

    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ConfigureMqttEvents();
        try
        {
            var factory = new MqttFactory();
            _client = factory.CreateMqttClient();
            await ConnectAsync(stoppingToken);
            Console.WriteLine("MQTT Connected Successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MQTT Connection Failed: {ex}");
        }
        while (!stoppingToken.IsCancellationRequested) await Task.Delay(1000, stoppingToken);
    }
    private async Task ConfigureMqttEvents()
    {
        _client.ConnectedAsync += async e =>
        {
            _logger.LogInformation("MQTT CONNECTED");
            await SubscribeTopics();
        };
        _client.DisconnectedAsync += async e =>
        {
            _logger.LogWarning("MQTT DISCONNECTED");
            await ReconnectAsync();
        };
        // MESSAGE RECEIVED
        _client.ApplicationMessageReceivedAsync += async e =>
        {
            try
            {
                var topic = e.ApplicationMessage.Topic;
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                _logger.LogInformation("MQTT MESSAGE RECEIVED | Topic: {Topic}",topic);
                if (topic.EndsWith("/sync"))
                {
                    using var scope = _scopeFactory.CreateScope();
                    var deviceService = scope.ServiceProvider.GetRequiredService<IDeviceService>();
                    deviceService.SyncDevices();
                }
                // DEVICE STATUS
                if (topic.EndsWith("/status")) _logger.LogInformation("DEVICE STATUS RECEIVED");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MQTT MESSAGE PROCESSING FAILED");
            }
        };
        await Task.CompletedTask;
    }
    // CONNECT TO MQTT BROKER
    public async Task ConnectAsync(CancellationToken cancellationToken)
    {
        if (_configuration == null)
            throw new Exception("Configuration is NOT injected into MqttService");

        var host = _configuration["Mqtt:Host"];
        var portString = _configuration["Mqtt:Port"];
        var username = _configuration["Mqtt:Username"];
        var password = _configuration["Mqtt:Password"];

        if (string.IsNullOrWhiteSpace(host))
            throw new Exception("MQTT Host is missing in configuration");

        if (!int.TryParse(portString, out var port))
            throw new Exception("MQTT Port is invalid or missing");

        if (string.IsNullOrWhiteSpace(username))
            throw new Exception("MQTT Username is missing");

        if (string.IsNullOrWhiteSpace(password))
            throw new Exception("MQTT Password is missing");

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(host, port)
            .WithCredentials(username, password)
            .WithCleanSession(false)
            .WithKeepAlivePeriod(TimeSpan.FromSeconds(30))
            .Build();

        await _client.ConnectAsync(options, cancellationToken);
    }
    // AUTO RECONNECT
    private async Task ReconnectAsync()
    {
        while (!_client.IsConnected)
        {
            try
            {
                _logger.LogInformation("RECONNECTING MQTT...");
                await Task.Delay(5000);
                await ConnectAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MQTT RECONNECT FAILED");
            }
        }
    }    
    private async Task SubscribeTopics()
    {
        var options = new MqttClientSubscribeOptionsBuilder().WithTopicFilter(f =>
        {
            f.WithTopic("controller/state");
            f.WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce);
        }).WithTopicFilter(f =>
        {
            f.WithTopic("controller/sync");
            f.WithQualityOfServiceLevel(
            MqttQualityOfServiceLevel.AtLeastOnce);
        }).Build();
        await _client.SubscribeAsync(options);
        _logger.LogInformation("MQTT TOPICS SUBSCRIBED");
    }
    private async Task PublishCommand(GetDeviceDto obj)
    {
        var topic = $"controller/command";
        await PublishAsync(topic, obj, true);
        _logger.LogInformation("COMMAND SENT");
    }
    private async Task PublishSync(List<GetDeviceDto> obj)
    {
        var topic = $"controller/syncDevice";
        await PublishAsync(topic, obj, true);
        _logger.LogInformation("SYNC SENT");
    }
    // GENERIC MQTT PUBLISHER
    private async Task PublishAsync(string topic, object payload, bool retain = false)
    {
        var json = JsonSerializer.Serialize(payload);
        if (_client == null)
            throw new InvalidOperationException("MQTT client not initialized.");

        if (!_client.IsConnected)
        {
            await ConnectAsync(CancellationToken.None);
        }

        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(JsonSerializer.Serialize(payload))
            .WithRetainFlag(retain)
            .Build();

        await _client.PublishAsync(message);
        _logger.LogInformation("MQTT MESSAGE PUBLISHED | Topic: {Topic}", topic);
    }
}