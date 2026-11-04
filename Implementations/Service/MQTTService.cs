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

    private readonly IMqttClient _client;

    public MqttService(
        IConfiguration configuration,
        IServiceScopeFactory scopeFactory,
        ILogger<MqttService> logger)
    {
        _configuration = configuration;
        _scopeFactory = scopeFactory;
        _logger = logger;

        var factory = new MqttFactory();

        // Create the MQTT client ONCE.
        _client = factory.CreateMqttClient();

        ConfigureMqttEvents();
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation(
                "HAUTO MQTT SERVICE STARTING...");

            await ConnectAsync(stoppingToken);

            _logger.LogInformation(
                "HAUTO MQTT SERVICE CONNECTED SUCCESSFULLY");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "MQTT INITIAL CONNECTION FAILED");
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(
                TimeSpan.FromSeconds(40),
                stoppingToken);
        }
    }

    private void ConfigureMqttEvents()
    {
        // =========================
        // CONNECTED
        // =========================

        _client.ConnectedAsync += async e =>
        {
            _logger.LogInformation(
                "MQTT CONNECTED");

            try
            {
                await SubscribeTopics();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "MQTT SUBSCRIPTION FAILED");
            }
        };


        // =========================
        // DISCONNECTED
        // =========================

        _client.DisconnectedAsync += async e =>
        {
            _logger.LogWarning(
                "MQTT DISCONNECTED | Reason: {Reason}",
                e.ReasonString);

            await ReconnectAsync();
        };


        // =========================
        // MESSAGE RECEIVED
        // =========================

        _client.ApplicationMessageReceivedAsync += async e =>
        {
            try
            {
                var topic = e.ApplicationMessage.Topic;

                var payload =
                    e.ApplicationMessage.PayloadSegment.Count > 0
                        ? Encoding.UTF8.GetString(
                            e.ApplicationMessage.PayloadSegment)
                        : string.Empty;

                _logger.LogInformation(
                    "MQTT MESSAGE RECEIVED | Topic: {Topic} | Payload: {Payload}",
                    topic,
                    payload);


                // ESP32 REQUESTS DEVICE SYNC
                if (topic.Contains("controller/sync"))
                {
                    _logger.LogInformation(
                        "SYNC REQUEST RECEIVED FROM ESP32");

                    using var scope =
                        _scopeFactory.CreateScope();

                    var deviceService =
                        scope.ServiceProvider
                            .GetRequiredService<IDeviceService>();

                    //await deviceService.SyncDevices();

                    _logger.LogInformation(
                        "SYNC DEVICES COMPLETED");
                }


                // DEVICE STATUS
                if (topic.EndsWith("/status"))
                {
                    _logger.LogInformation(
                        "DEVICE STATUS RECEIVED");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "MQTT MESSAGE PROCESSING FAILED");
            }
        };
    }


    // =========================
    // CONNECT
    // =========================

    public async Task ConnectAsync(CancellationToken cancellationToken)
    {
        var host = _configuration["Mqtt:Host"];
        var portString = _configuration["Mqtt:Port"];
        var username = _configuration["Mqtt:Username"];
        var password = _configuration["Mqtt:Password"];

        if (string.IsNullOrWhiteSpace(host))
            throw new Exception("MQTT Host is missing");

        if (!int.TryParse(portString, out var port))
            throw new Exception("MQTT Port is invalid");

        if (string.IsNullOrWhiteSpace(username))
            throw new Exception("MQTT Username is missing");

        if (string.IsNullOrWhiteSpace(password))
            throw new Exception("MQTT Password is missing");

        if (_client.IsConnected)
        {
            _logger.LogInformation(
                "MQTT CLIENT ALREADY CONNECTED");

            return;
        }

        var clientId = "Hauto-Backend";

        var options = new MqttClientOptionsBuilder()
            .WithClientId(clientId)
            .WithTcpServer(host, port)
            .WithCredentials(username, password)
            .WithCleanSession(true)
            .WithKeepAlivePeriod(TimeSpan.FromSeconds(30))
            .Build();

        _logger.LogInformation(
            "MQTT CONNECTING | ClientId={ClientId} | Host={Host} | Port={Port}",
            clientId,
            host,
            port);

        //await _client.ConnectAsync(
            //options,
            //cancellationToken);

        _logger.LogInformation(
            "MQTT CONNECTION STATE | Connected={Connected}",
            _client.IsConnected);
    }


    // =========================
    // RECONNECT
    // =========================

    private async Task ReconnectAsync()
    {
        while (!_client.IsConnected)
        {
            try
            {
                _logger.LogInformation(
                    "RECONNECTING MQTT...");

                await Task.Delay(
                    TimeSpan.FromSeconds(40));

                await ConnectAsync(
                    CancellationToken.None);

                _logger.LogInformation(
                    "MQTT RECONNECTED SUCCESSFULLY");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "MQTT RECONNECT FAILED");
            }
        }
    }


    // =========================
    // SUBSCRIBE
    // =========================

    private async Task SubscribeTopics()
    {
        var options =
            new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter(f =>
                {
                    f.WithTopic(
                        "controller/sync");

                    f.WithQualityOfServiceLevel(
                        MqttQualityOfServiceLevel
                            .AtLeastOnce);
                })
                .Build();


        var response =
            await _client.SubscribeAsync(options);


        foreach (var item in response.Items)
        {
            _logger.LogInformation(
                "MQTT SUBSCRIBED | Topic: {Topic} | Result: {ResultCode}",
                item.TopicFilter.Topic,
                item.ResultCode);
        }
    }


    // =========================
    // COMMAND
    // =========================

    public async Task PublishCommand(
        GetDeviceDto obj)
    {
        const string topic =
            "controller/command";

        await PublishAsync(
            topic,
            obj,
            true);

        _logger.LogInformation(
            "COMMAND SENT");
    }


    // =========================
    // DEVICE SYNC
    // =========================

    public async Task PublishSync(
        List<GetDeviceDto> obj)
    {
        const string topic =
            "controller/syncDevice";

        _logger.LogInformation(
            "SYNCING {Count} DEVICES",
            obj.Count);

        await PublishAsync(
            topic,
            obj,
            true);

        _logger.LogInformation(
            "SYNC SENT");
    }


    // =========================
    // GENERIC PUBLISH
    // =========================

    private async Task PublishAsync(
        string topic,
        object payload,
        bool retain = false)
    {
        if (!_client.IsConnected)
        {
            _logger.LogWarning(
                "MQTT CLIENT NOT CONNECTED. CONNECTING...");

            await ConnectAsync(
                CancellationToken.None);
        }


        var json =
            JsonSerializer.Serialize(payload);


        var message =
            new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(json)
                .WithQualityOfServiceLevel(
                    MqttQualityOfServiceLevel
                        .AtLeastOnce)
                .WithRetainFlag(retain)
                .Build();


        //await _client.PublishAsync(message);


        _logger.LogInformation(
            "MQTT MESSAGE PUBLISHED | Topic: {Topic}",
            topic);
    }
}