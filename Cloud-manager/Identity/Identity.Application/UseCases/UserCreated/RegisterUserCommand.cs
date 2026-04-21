using Identity.Domain.ValueObjects.Result;
using MediatR;

namespace Identity.Application.UseCases.UserCreated;

public record RegisterUserCommand(string Email, string Password) : IRequest<Result<string>>;