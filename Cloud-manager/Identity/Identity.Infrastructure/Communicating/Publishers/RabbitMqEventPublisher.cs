using System.Text;
using System.Text.Json;
using Identity.Domain.Events.Publisher;
using RabbitMQ.Client;

namespace Identity.Infrastructure.Communicating.Publishers;

public sealed class RabbitMqEventPublisher : IEventPublisher
{
    private readonly IConnection _connection;
    private readonly SemaphoreSlim _lock = new(1, 1); // 1 chanel 
    private readonly string _exchange;
    private IChannel? Channel { get; set; }
    
    
    public RabbitMqEventPublisher(IConnection connection, string exchange = "identity-events")
    {
        _connection = connection;
        _exchange = exchange;
    }

    public async Task<IChannel> GetChannelAsync()
    {
        if (Channel is { IsOpen: true }) return Channel;
        
        await _lock.WaitAsync();
        try
        {
            if (Channel is { IsOpen: true }) return Channel;
            
               
            var options = new CreateChannelOptions(publisherConfirmationsEnabled: true, // turn ACK on broker side 
                publisherConfirmationTrackingEnabled: true, // track this acknowlages
                outstandingPublisherConfirmationsRateLimiter: new ThrottlingRateLimiter(maxConcurrentCalls: 100 ) // process speed limit to  
            );
            
            Channel = await _connection.CreateChannelAsync(options); // creating chanel(virtual connection inside one TCP con) 1 долгоживущий канал, or await using
            // fire and forget
            
            await Channel.ExchangeDeclareAsync(
                _exchange,
                ExchangeType.Topic, 
                durable: true);
            
            return Channel;
        }
        finally
        {
            _lock.Release(); // lazy release object
        }
    }
    
    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class // problem srp create chanel + publish
    {
        var channel = await GetChannelAsync();
        string queueName = @event.GetType().Name.ToLower();
        
        var json = JsonSerializer.Serialize(@event); 
        var body = Encoding.UTF8.GetBytes(json);

        var prop = new BasicProperties()
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent,
            MessageId = Guid.NewGuid().ToString(),
            Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
        };
            
        try
        {
            await channel.BasicPublishAsync(
                _exchange,
                queueName,
                mandatory: true, // support messages without queue 
                prop,
                body);
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync($"{DateTime.Now} [ERROR] nack saw or publish exception: {e}");
            throw;
        }
    }
}