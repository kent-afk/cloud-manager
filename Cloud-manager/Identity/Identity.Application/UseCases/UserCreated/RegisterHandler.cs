using Identity.Domain.Entities.PasswordHasher;
using Identity.Domain.Entities.UserRepository;
using Identity.Domain.Entities.Users;
using Identity.Domain.Events;
using Identity.Domain.Events.Publisher;
using Identity.Domain.ValueObjects.Email;
using Identity.Domain.ValueObjects.Result;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Identity.Application.UseCases.UserCreated;

public class RegisterHandler : IRequestHandler<RegisterUserCommand, Result<string>>
{
    private readonly ILogger<RegisterHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEventPublisher  _eventPublisher;
    
    public RegisterHandler(
        ILogger<RegisterHandler> logger,
        IPasswordHasher passwordHasher,
        IUserRepository userRepository,
        IEventPublisher eventPublisher)
    {
        _logger = logger;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<Result<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
            var email = Email.Create(request.Email).Value;
            if (email is null)
                return Result<string>.Failure("Email address does not exist");
            
            var exist = await _userRepository.GetByEmailAsync(email, cancellationToken);
            
            if (exist is not null)
            {
                return Result<string>.Failure("User Already Exist");
            }

            if (request.Password == string.Empty)
            { 
                return Result<string>.Failure("Password or Email does not match");
            }

            var passwordHash = _passwordHasher.Hash(request.Password);

            var id = Guid.NewGuid();

            var user = new RegularUser(email, passwordHash, id);
            
            await _userRepository.AddAsync(user, cancellationToken);
            
            await _eventPublisher.PublishAsync(
                new User_Registered_Event(id, email.Value), 
                cancellationToken);
            
            _logger.LogError($"User with email {email} created");
            
            return  Result<string>.Success("User created");
    }   
}