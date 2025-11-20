public interface IKafkaProducerService
{
    Task SendMessageAsync(string message);
}