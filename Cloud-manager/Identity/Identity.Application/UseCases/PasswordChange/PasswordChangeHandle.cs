using Identity.Domain.ValueObjects.Result;
using MediatR;

namespace Identity.Application.UseCases.PasswordChange;

public class PasswordChangeHandle : IRequestHandler<PasswordChangeCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PasswordChangeCommand request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}