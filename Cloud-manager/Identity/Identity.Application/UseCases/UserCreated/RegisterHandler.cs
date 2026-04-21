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
        ILogger<RegisterHandler> logger, IPasswordHasher passwordHasher, IUserRepository userRepository, IEventPublisher eventPublisher)
    {
        _logger = logger;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<Result<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
            var email = Email.Create(request.Email);
          
            var exist = await _userRepository.GetByEmailAsync(email);
            
            if (exist is not null)
            {
                _logger.LogError($"User with email {email} already exist");
                return new Result<string>(false, ErrorMessage: "User Already Exist");
            }

            if (request.Password == string.Empty)
            { 
                _logger.LogError($"User with email {email} does not have a password");
                return new Result<string>(false, ErrorMessage: "User need a password");
            }

            var passwordHash = _passwordHasher.Hash(request.Password);

            var id = Guid.NewGuid();
            
            await _userRepository.AddAsync(new RegularUser(email, passwordHash, id), cancellationToken);
            
            await _eventPublisher.PublishAsync(
                new User_Registered_Event(id, email.Value), 
                cancellationToken);
            
            return new Result<string>(true,
                _userRepository.GetByEmailAsync(email).ToString()); // TODO
    }   
}