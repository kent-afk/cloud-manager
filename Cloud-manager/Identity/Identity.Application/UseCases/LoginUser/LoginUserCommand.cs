using Identity.Domain.Events;
using Identity.Domain.ValueObjects.Result;
using MediatR;

namespace Identity.Application.UseCases.LoginUser;

public record LoginUserCommand(Loging_User_Event Event) : IRequest<Result<string>>;