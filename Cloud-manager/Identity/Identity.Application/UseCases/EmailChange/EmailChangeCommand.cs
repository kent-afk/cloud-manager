using Identity.Domain.ValueObjects.Result;
using MediatR;

namespace Identity.Application.UseCases.EmailChange;

public record EmailChangeCommand(string Email, string Password, string NewEmail) :  IRequest<Result<string>>;