using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Prometheus.Client;
using Prometheus.Client.MetricServer;


namespace Identity.Application.UseCases.UserCreated;

public class ProcessUserCreatedHandler : IRequestHandler<ProcessUserCreatedCommand>
{
    private IHubContext _context;
    private IMetricFamily<ICounter> _usersCounter;
    private IEmailSender _emailSender;
    ILogger<ProcessUserCreatedHandler> _logger;

    public ProcessUserCreatedHandler(
        IHubContext context, 
        IMetricFactory metricsFactory,
        IEmailSender emailSender,
        ILogger<ProcessUserCreatedHandler> logger)
    {
        _context = context;
        _usersCounter = metricsFactory.CreateCounter(
            "identity-application-user-created",
            "Total number of users created",
            labelNames: ["source"]
        );
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task Handle(ProcessUserCreatedCommand request, CancellationToken cancellationToken)
    {
        var userId = request.Event.ClientId.ToString();
        
        await _context.Clients.User(userId)
            .SendAsync("UserCreated", new { 
                Message = "User created successfully",
                ClientId = userId
            }, cancellationToken); // User Group
        
        _usersCounter.Inc();
        _logger.LogInformation($"User {userId} Created: Successfully");
    }
}