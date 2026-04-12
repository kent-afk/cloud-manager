using Identity.Application.Ports.JwtGenerator;
using Identity.Domain.Entities.UserRepository;
using Identity.Domain.ValueObjects.Result;
using MediatR;

namespace Identity.Application.UseCases.LoginUser;

public class LoginHandler : IRequestHandler<LoginUserCommand, Result<string>>
{
    private IUserRepository _userRepository;
    private IJwtGenerator _jwtGenerator;
    
    public LoginHandler(IUserRepository userRepository, IJwtGenerator jwtGenerator)
    {
        _userRepository = userRepository;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<Result<string>> Handle(LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        bool userExist = await _userRepository.UserExistAsync(request.Event.Email, request.Event.HashPassword);

        if (!userExist)
        {
            await Console.Error.WriteLineAsync("User not found");
            throw new KeyNotFoundException("User not found");
        }
        
        string token = _jwtGenerator.GenerateToken(request.Event.Email, request.Event.HashPassword);
        
        return new Result<string>(true, token);
    }
}