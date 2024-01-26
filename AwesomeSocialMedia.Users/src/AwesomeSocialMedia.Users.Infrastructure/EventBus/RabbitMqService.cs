using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace AwesomeSocialMedia.Users.Infrastructure.EventBus;
public class RabbitMqService : IEventBus
{
    private readonly IModel _channel;
    private const string Exchange = "update";
    public RabbitMqService()
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        var connection = connectionFactory.CreateConnection("update-user.publisher");

        _channel = connection.CreateModel();

        _channel.ExchangeDeclare(Exchange, "direct", true, false);
    }
    public void Publish<T>(T @event)
    {
        //var routingKey = @event.GetType().Name.ToDashCase();
        var routingKey = "user-created";
        var json = JsonSerializer.Serialize(@event);
        var bytes = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(Exchange, routingKey, null, bytes);
    }
}
