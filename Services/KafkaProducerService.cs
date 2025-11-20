using Confluent.Kafka;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _topic;

    public KafkaProducerService(IConfiguration configuration)
    {
        var kafkaSettings = configuration.GetSection("Kafka");
        _topic = "User";

        var config = new ProducerConfig
        {
            BootstrapServers = kafkaSettings["servers"],
            SecurityProtocol = SecurityProtocol.SaslSsl,
            SaslMechanism = SaslMechanism.Plain,
            SaslUsername = kafkaSettings["sasl_username"],
            SaslPassword = kafkaSettings["sasl_password"]
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task SendMessageAsync(string message)
    {
        await _producer.ProduceAsync(_topic, new Message<Null, string>
        {
            Value = message
        });
    }
}


