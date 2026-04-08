using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Identity.Application.UseCases.UserCreated;
using Identity.Domain.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Identity.Infrastructure.Communicating.Consumers;

public class UserCreatedConsumer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnection _connection;

    public UserCreatedConsumer(IServiceProvider serviceProvider, IConnection connection)
    {
        _serviceProvider = serviceProvider;
        _connection = connection;
    }

    public async Task HandleMessage(BasicDeliverEventArgs deliverEventArgs)
    {
        var message = Encoding.UTF8.GetString(deliverEventArgs.Body.ToArray());
        var @event = JsonSerializer.Deserialize<User_Registered_Event>(message);

        await using var scope = _serviceProvider.CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        
        await mediator.Send(new ProcessUserCreatedCommand(@event));
    }
}