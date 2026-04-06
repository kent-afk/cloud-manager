using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Identity.Domain.Events.Publisher;
using RabbitMQ.Client;

namespace Identity.Infrastructure.Communicating;

public class RabbitMQEventPublisher : IEventPublisher
{
    private readonly IConnection _connection;

    public RabbitMQEventPublisher(IConnection connection)
    {
        _connection = connection;
    }


    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
    {
        string route = @event.GetType().Name.ToLower();
        await using var channel = await _connection.CreateChannelAsync(); // creating chanel(virtual connection inside one TCP con)
        // fire and forget
        
        var json = JsonSerializer.Serialize(@event); 
        var body = Encoding.UTF8.GetBytes(json);
        
        await channel.BasicPublishAsync(
            route, // send queue
            route,
            body);
            
    }
}