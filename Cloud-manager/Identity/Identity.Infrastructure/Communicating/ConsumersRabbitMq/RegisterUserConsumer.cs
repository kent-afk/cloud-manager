using System.Text;
using System.Text.Json;
using Identity.Application.UseCases.UserCreated;
using Identity.Domain.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;

namespace Identity.Infrastructure.Communicating.ConsumersRabbitMq;

public sealed class RegisterUserConsumer : IConsumer
{
    private readonly IServiceProvider _serviceProvider;

    public RegisterUserConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ConsumeAsync(BasicDeliverEventArgs deliverEventArgs)
    {
        var message = Encoding.UTF8.GetString(deliverEventArgs.Body.ToArray());
        var @event = JsonSerializer.Deserialize<User_Registered_Event>(message);

        await using var scope = _serviceProvider.CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        
        await mediator.Send(new RegisterUserCommand(@event ?? throw new InvalidOperationException()));
    }
}