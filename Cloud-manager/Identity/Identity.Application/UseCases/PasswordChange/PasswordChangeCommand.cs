using Identity.Domain.ValueObjects.Result;
using MediatR;

namespace Identity.Application.UseCases.PasswordChange;

public record PasswordChangeCommand(string Email,string OldPassword ,string NewPassword) :  IRequest<Result<string>>;