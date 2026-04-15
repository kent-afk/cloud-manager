using Identity.Domain.Events;
using Identity.Domain.ValueObjects.Result;
using MediatR;

namespace Identity.Application.UseCases.PasswordChange;

public record PasswordChangeCommand(Password_Change_Event Event) :  IRequest<Result<string>>;