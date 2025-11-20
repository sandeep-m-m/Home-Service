// using Confluent.Kafka;

// public class KafkaConsumerBackgroundService : BackgroundService
// {
//     private readonly IConfiguration _config;

//     public KafkaConsumerBackgroundService(IConfiguration config)
//     {
//         _config = config;
//     }

//     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//     {
//         var settings = _config.GetSection("Kafka");

//         var consumerConfig = new ConsumerConfig
//         {
//             BootstrapServers = settings["BootstrapServers"],
//             SecurityProtocol = SecurityProtocol.SaslSsl,
//             SaslMechanism = SaslMechanism.Plain,
//             SaslUsername = settings["SaslUsername"],
//             SaslPassword = settings["SaslPassword"],
//             GroupId = "my-api-consumer",
//             AutoOffsetReset = AutoOffsetReset.Earliest
//         };

//         using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
//         consumer.Subscribe(settings["Topic"]);

//         while (!stoppingToken.IsCancellationRequested)
//         {
//             var cr = consumer.Consume(stoppingToken);
//             Console.WriteLine($"Received from Kafka: {cr.Message.Value}");
//         }
//     }
// }
