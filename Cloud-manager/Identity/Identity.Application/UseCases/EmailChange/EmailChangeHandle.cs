using Identity.Domain.Entities.PasswordHasher;
using Identity.Domain.Entities.UserRepository;
using Identity.Domain.Events;
using Identity.Domain.Events.Publisher;
using Identity.Domain.ValueObjects.Email;
using Identity.Domain.ValueObjects.Result;
using MediatR;

namespace Identity.Application.UseCases.EmailChange;

public class EmailChangeHandle : IRequestHandler<EmailChangeCommand, Result<string>>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEventPublisher _eventPublisher;
    private readonly IUserRepository _userRepository;

    public EmailChangeHandle(IPasswordHasher passwordHasher, IEventPublisher eventPublisher, IUserRepository userRepository)
    {
        _passwordHasher = passwordHasher;
        _eventPublisher = eventPublisher;
        _userRepository = userRepository;
    }

    public async Task<Result<string>> Handle(EmailChangeCommand request,
        CancellationToken cancellationToken) // TODO
    {
        var currentEmail = Email.Create(request.Email);

        if (currentEmail.Value == null)
        {
            return Result<string>.Failure("Email is not valid.");
        }
        
        var user = await _userRepository.GetByEmailAsync(currentEmail.Value, cancellationToken);

        if (user == null)
        {
            return Result<string>.Failure("Password or Email does not match");
        }

        bool verifyUser = _passwordHasher.Verify(request.Password, user.HashPassword.Value); // maybe deligate this check in another class to remain srp
        
        if (!verifyUser)
            return Result<string>.Failure("Password or Email does not match");
        
        var newEmail = Email.Create(request.NewEmail);
        
        if (newEmail.Value == null)
        {
            return Result<string>.Failure("Email is not valid.");
        }
        
        user.ChangeEmail(newEmail.Value);
        
        await _userRepository.UpdateAsync(user, cancellationToken);

        await _eventPublisher.PublishAsync(new Email_Changed_Domain_Event(newEmail.Value),
            cancellationToken);
        
        return Result<string>.Success("Email changed");
    }
}