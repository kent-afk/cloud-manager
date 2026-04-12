using Identity.Domain.Events;
using MediatR;

namespace Identity.Application.UseCases.UserCreated;

public record RegisterUserCommand(User_Registered_Event Event) : IRequest;