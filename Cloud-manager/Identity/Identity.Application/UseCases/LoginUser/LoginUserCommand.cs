using Identity.Domain.ValueObjects.Result;
using MediatR;

namespace Identity.Application.UseCases.LoginUser;

public record LoginUserCommand(string Email, string Password) : IRequest<Result<string>>;