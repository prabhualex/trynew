To consume Kafka messages using a secured Bootstrap Server and settings defined in an `appsettings.json` file, you can follow these steps:

1. * *Create a Worker Service Project**: In Visual Studio 2022, create a new Worker Service project.

2. * *Install Required NuGet Packages**: Add the following NuGet packages to the project:

    - `Confluent.Kafka` for Kafka support.
    - `Microsoft.Extensions.Hosting` and `Microsoft.Extensions.Logging` for the worker service framework and logging.
    - `Microsoft.Extensions.Configuration.Json` for reading the configuration from `appsettings.json`.

3. * *Configure `appsettings.json`**: Add your provided configuration to `appsettings.json`:

    ```json
    {
            "AllowedHosts": "*",
        "Bam": {
                "BamAuthUrl": "https://bamlatform&redirectURL=http://dummy",
            "TokenExpirationInMinutes": 60
        },
        "Logging": {
                "LogLevel": {
                    "Default": "Information",
                "Microsoft.Hosting.Lifetime": "Information"
                }
            },
        "ConsumerSettings": {
                "BootstrapServers": "ldndsr000092",
            "GroupId": "KafkaPSample-1",
            "EnableAutoCommit": true,
            "SessionTimeoutMs": 6000,
            "MessageMaxBytes": 90000000,
            "StatisticsIntervalMs": 10000,
            "AutoOffsetReset": "Earliest",
            "SecurityProtocol": "Saslssl",
            "SaslMechanism": "OAuthBearer",
            "EnableSslCertificateVerification": true,
            "SslCaLocation": "C:\\User\\Dev\\server.cer.pem",
            "Topic": "tc11_cmcv_dev_opics_fct_cashmsg_0"
        }
        }
    ```

4. * *Create a Worker Service**: Create a worker service class (`KafkaConsumerWorker`) that inherits from `BackgroundService` and contains the logic to consume Kafka messages.

5. **Configure `KafkaConsumerWorker`**: In `KafkaConsumerWorker`, configure the Kafka consumer with settings from `appsettings.json` and consume messages. Handle authentication with the `BamAuthUrl` as necessary.

6. **Register the Worker Service**: Register the worker service (`KafkaConsumerWorker`) in `Program.cs` using the `Host` builder.

Here is the code:

-**`KafkaConsumerWorker.cs`**:

    ```csharp
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class KafkaConsumerWorker : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<KafkaConsumerWorker> _logger;

    private IConsumer<string, string> _consumer;
    private string _topic;

    public KafkaConsumerWorker(IConfiguration configuration, ILogger<KafkaConsumerWorker> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("KafkaConsumerWorker starting.");

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _configuration["ConsumerSettings:BootstrapServers"],
            GroupId = _configuration["ConsumerSettings:GroupId"],
            EnableAutoCommit = bool.Parse(_configuration["ConsumerSettings:EnableAutoCommit"]),
            SessionTimeoutMs = int.Parse(_configuration["ConsumerSettings:SessionTimeoutMs"]),
            StatisticsIntervalMs = int.Parse(_configuration["ConsumerSettings:StatisticsIntervalMs"]),
            AutoOffsetReset = (AutoOffsetReset)Enum.Parse(typeof(AutoOffsetReset), _configuration["ConsumerSettings:AutoOffsetReset"]),
            SecurityProtocol = (SecurityProtocol)Enum.Parse(typeof(SecurityProtocol), _configuration["ConsumerSettings:SecurityProtocol"]),
            SaslMechanism = (SaslMechanism)Enum.Parse(typeof(SaslMechanism), _configuration["ConsumerSettings:SaslMechanism"]),
            SslCaLocation = _configuration["ConsumerSettings:SslCaLocation"],
            EnableSslCertificateVerification = bool.Parse(_configuration["ConsumerSettings:EnableSslCertificateVerification"]),
            SaslOauthbearerTokenCallback = callback => GetAuthToken() // Custom OAuth Bearer token callback function.
        };

        _topic = _configuration["ConsumerSettings:Topic"];

        _consumer = new ConsumerBuilder<string, string>(consumerConfig)
                        .SetValueDeserializer(Deserializers.Utf8)
                        .Build();

        _consumer.Subscribe(_topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(stoppingToken);

                _logger.LogInformation($"Received message: Key = {consumeResult.Message.Key}, Value = {consumeResult.Message.Value}");

                // Add your message processing logic here.
            }
        }
        catch (OperationCanceledException)
        {
            // This is expected when the task is cancelled.
            _logger.LogInformation("Stopping KafkaConsumerWorker...");
        }
        finally
        {
            _consumer.Close();
            _consumer.Dispose();
        }
    }

    // Function to get the OAuth Bearer token.
    private string GetAuthToken()
    {
        // Add your logic to fetch the token from the BamAuthUrl.
        string bamAuthUrl = _configuration["Bam:BamAuthUrl"];

        // Here you would make a request to the BamAuthUrl to retrieve an OAuth bearer token.
        // For example, using HttpClient.

        // In this example, we will just return a dummy token for demonstration purposes.
        // Replace this with your actual logic to fetch the token.
        return "dummy-token";
    }
}
    ```

-**`Program.cs`**:

    ```csharp
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<KafkaConsumerWorker>();
            });
}
    ```

7. * *Build and Run**: Build and run the application. The worker service will consume Kafka messages based on the provided configuration.

Please make sure to handle error scenarios and network failures appropriately in your implementation. Adjust the code according to your needs, including error handling, logging, and authentication token management.