using Identity.Domain.ValueObjects.Result;
using MediatR;

namespace Identity.Application.UseCases.EmailChange;

public class EmailChangeHandle : IRequestHandler<EmailChangeCommand, Result<string>>
{
    public async Task<Result<string>> Handle(EmailChangeCommand request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}