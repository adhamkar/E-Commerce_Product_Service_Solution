using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text.Json;

namespace BusinessLogic.RabbitMQ;

public class Publisher : IPublisher, IDisposable
{
    private readonly IConfiguration configuration;
    private readonly IChannel channel;
    private readonly IConnection conn;
    public Publisher(IConfiguration configuration)
    {
        this.configuration = configuration;

        string hostName = "localhost"; 
        string userName = "guest";
        string password = "guest";
        string port = "5672";

        ConnectionFactory connection = new ConnectionFactory
        {
            HostName = hostName,
            UserName = userName,
            Password = password,
            Port = int.Parse(port)
        };

        conn = connection.CreateConnectionAsync().GetAwaiter().GetResult();

        channel = conn.CreateChannelAsync().GetAwaiter().GetResult();
    }

    public void Dispose()
    {
        channel.Dispose();
        conn.Dispose();
    }

    public async Task Pusblish<T>(string routingKey, T message)
    {
        string messageJson = JsonSerializer.Serialize(message);
        byte[] msgBody = System.Text.Encoding.UTF8.GetBytes(messageJson);

        //Create Exchange
        string exchangeName = "products.exchange";

        await channel.ExchangeDeclareAsync(exchange:
            exchangeName,
            type: ExchangeType.Direct,
            durable: true);

        //Publish Message
        var props = new BasicProperties();
        await channel.BasicPublishAsync<BasicProperties>(
            exchange: exchangeName,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: props,
            body: msgBody
        );
    }
}
