using Identity.Application.Ports.JwtGenerator;
using Identity.Domain.Entities.PasswordHasher;
using Identity.Domain.Entities.UserRepository;
using Identity.Domain.Events;
using Identity.Domain.Events.Publisher;
using Identity.Domain.ValueObjects.Email;
using Identity.Domain.ValueObjects.Result;
using MediatR;

namespace Identity.Application.UseCases.LoginUser;

public class LoginHandler : IRequestHandler<LoginUserCommand, Result<string>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEventPublisher  _eventPublisher;
    
    public LoginHandler(
        IUserRepository userRepository,
        IJwtGenerator jwtGenerator,
        IPasswordHasher passwordHasher,
        IEventPublisher eventPublisher)
    {
        _userRepository = userRepository;
        _jwtGenerator = jwtGenerator;
        _passwordHasher = passwordHasher;
        _eventPublisher = eventPublisher;
    }

    public async Task<Result<string>> Handle(LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        var email = Email.Create(request.Email).Value;
        
        if (email is null)
        {
            return Result<string>.Failure("Email is not valid.");
        }
        
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        
        if (user == null)
        {
            return Result<string>.Failure("Invalid password or email");
        }
        
        bool verify = _passwordHasher.Verify(request.Password, user.HashPassword.Value);
        if (!verify)
        {
            return  Result<string>.Failure("Invalid password or email");
        }

        string token = _jwtGenerator.GenerateToken(user);
        
        await _eventPublisher.PublishAsync(
            new Loging_User_Event(user.UserId, user.Email.Value),
            cancellationToken);
        
        return Result<string>.Success(token);
    }
}