using Identity.Domain.Events;
using MediatR;

namespace Identity.Application.UseCases.UserCreated;

public record ProcessUserCreatedCommand(User_Registered_Event Event) : IRequest;