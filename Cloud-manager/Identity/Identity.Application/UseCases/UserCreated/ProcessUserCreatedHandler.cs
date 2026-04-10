using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Identity.Application.UseCases.UserCreated;

public class ProcessUserCreatedHandler : IRequestHandler<ProcessUserCreatedCommand>
{
    public async Task Handle(ProcessUserCreatedCommand request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException(); //TODO
    }
}