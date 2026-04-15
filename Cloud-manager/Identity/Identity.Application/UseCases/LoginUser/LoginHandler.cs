using Identity.Application.Ports.JwtGenerator;
using Identity.Domain.Entities.PasswordHasher;
using Identity.Domain.Entities.UserRepository;
using Identity.Domain.ValueObjects.Result;
using MediatR;

namespace Identity.Application.UseCases.LoginUser;

public class LoginHandler : IRequestHandler<LoginUserCommand, Result<string>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IPasswordHasher _passwordHasher;
    
    public LoginHandler(IUserRepository userRepository, IJwtGenerator jwtGenerator, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtGenerator = jwtGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<string>> Handle(LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Event.Email);
        if (user == null)
        {
            return new Result<string>(
                false,
                ErrorMessage:"Invalid password or email");
        }
        
        bool verify = _passwordHasher.Verify(request.Event.HashPassword, user.HashPassword.Value);
        if (!verify)
        {
            return new Result<string>(false,
                ErrorMessage:"Invalid password or email");
        }

        string token = _jwtGenerator.GenerateToken(user);
        
        return new Result<string>(true, token);
    }
}