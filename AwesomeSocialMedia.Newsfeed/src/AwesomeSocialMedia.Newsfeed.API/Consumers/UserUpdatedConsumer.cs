using AwesomeSocialMedia.Newsfeed.API.Core.Entities;
using AwesomeSocialMedia.Newsfeed.API.Core.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace AwesomeSocialMedia.Newsfeed.API.Consumers;

public class UserUpdatedConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IModel _channel;

    private const string Queue = "newsfeed.user-updated";
    private const string Exchange = "update";
    private const string RoutingKey = "user-created";
    public UserUpdatedConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        var connectionFactory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        var connection = connectionFactory.CreateConnection(Queue);

        _channel = connection.CreateModel();
        _channel.QueueDeclare(Queue, true, false, false, null);
        _channel.ExchangeDeclare(Exchange, "direct", true, false);
        _channel.QueueBind(Queue, Exchange, RoutingKey);
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (sender, eventArgs) =>
        {
            var contentArray = eventArgs.Body.ToArray();
            var json = Encoding.UTF8.GetString(contentArray);
            var @event = JsonConvert.DeserializeObject<UserUpdated>(json);

            _channel.BasicAck(eventArgs.DeliveryTag, false);
        };

        _channel.BasicConsume(Queue, false, consumer);

        return Task.CompletedTask;
    }

    public class UserUpdated
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public LocationInfoModel Location { get; set; }
        public ContactInfoModel Contact { get; set; }
    }

    public class LocationInfoModel
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }

    public class ContactInfoModel
    {
        public string Email { get; set; }
        public string Website { get; set; }
        public string PhoneNumber { get; set; }
    }
}
