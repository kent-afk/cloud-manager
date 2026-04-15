using System.Text;
using System.Text.Json;
using Identity.Application.UseCases.EmailChange;
using Identity.Domain.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;

namespace Identity.Infrastructure.Communicating.ConsumersRabbitMq;

public sealed class EmailChangedConsumer : IConsumer // maybe for reusability of code use <T>
{
    private readonly IServiceProvider _serviceProvider;

    public EmailChangedConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ConsumeAsync(BasicDeliverEventArgs deliverEventArgs)
    {
        var message = Encoding.UTF8.GetString(deliverEventArgs.Body.ToArray());
        var @event = JsonSerializer.Deserialize<Email_Changed_Domain_Event>(message);

        await using var scope = _serviceProvider.CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        
        await mediator.Send(new EmailChangeCommand(@event ?? throw new InvalidOperationException()));
    }
}