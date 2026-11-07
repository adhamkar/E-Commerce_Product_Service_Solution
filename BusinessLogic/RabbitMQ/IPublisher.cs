namespace BusinessLogic.RabbitMQ;

public interface IPublisher
{
    Task Pusblish<T>(string routingKey, T message);
}
