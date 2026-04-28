using Identity.Domain.Entities.PasswordHasher;
using Identity.Domain.Entities.UserRepository;
using Identity.Domain.Entities.Users;
using Identity.Domain.Events;
using Identity.Domain.Events.Publisher;
using Identity.Domain.ValueObjects.Email;
using Identity.Domain.ValueObjects.Result;
using MediatR;

namespace Identity.Application.UseCases.PasswordChange;

public class PasswordChangeHandle : IRequestHandler<PasswordChangeCommand, Result<string>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEventPublisher _eventPublisher;
    
    public PasswordChangeHandle(IPasswordHasher passwordHasher, IUserRepository userRepository, IEventPublisher eventPublisher)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _eventPublisher = eventPublisher;
    }
    
    public async Task<Result<string>> Handle(PasswordChangeCommand request,
        CancellationToken cancellationToken)
    {
        var email = Email.Create(request.Email).Value;
        
        if (email is null)
            return Result<string>.Failure("Password or Email address does not match");
        
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken); // ID -> Email 
        
        if (user == null)
            return Result<string>.Failure("Password or Email does not match");
        
        if (!_passwordHasher.Verify(request.OldPassword, user.HashPassword.Value))
        {
            return Result<string>.Failure("Password or Email does not match");
        }

        if (_passwordHasher.Verify(request.NewPassword, user.HashPassword.Value))
            return  Result<string>.Failure("Password repeated");

        var newPasswordHash = _passwordHasher.Hash(request.NewPassword);
        
        user.ChangePassword(newPasswordHash);

        await _userRepository.UpdateAsync(user, cancellationToken);

        var emailRequest = Email.Create(request.Email).Value;
        if (emailRequest is null)
            return Result<string>.Failure("Email address does not match");
        
        await _eventPublisher.PublishAsync(new Password_Change_Event(
                emailRequest,
                newPasswordHash.Value),
            cancellationToken);
        
        return Result<string>.Success("Password changed");
    }
}