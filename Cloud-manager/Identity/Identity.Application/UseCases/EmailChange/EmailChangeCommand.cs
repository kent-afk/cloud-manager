using Identity.Domain.Events;
using Identity.Domain.ValueObjects.Result;
using MediatR;

namespace Identity.Application.UseCases.EmailChange;

public record EmailChangeCommand(Email_Changed_Domain_Event Event) :  IRequest<Result<string>>;