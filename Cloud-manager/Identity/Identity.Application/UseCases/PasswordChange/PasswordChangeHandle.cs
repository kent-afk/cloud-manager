using Identity.Domain.Entities.PasswordHasher;
using Identity.Domain.Entities.UserRepository;
using Identity.Domain.ValueObjects.Result;
using MediatR;

namespace Identity.Application.UseCases.PasswordChange;

public class PasswordChangeHandle : IRequestHandler<PasswordChangeCommand, Result<string>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    
    public PasswordChangeHandle(IPasswordHasher passwordHasher, IUserRepository userRepository)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }

    public async Task<Result<string>> Handle(PasswordChangeCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Event.UserId); // ID -> Email 
        
        if (user == null)
            return new Result<string>(false, null, "User not found");

        if (_passwordHasher.Verify(request.Event.NewPassword, user.HashPassword.Value))
            return new Result<string>(false, null, "Password repeated");

        var newPasswordHash = _passwordHasher.Hash(request.Event.NewPassword);
        
        user.ChangePassword(newPasswordHash);

        await _userRepository.UpdateAsync(user);
        
        return new Result<string>(true, ErrorMessage:"Password changed");
    }
}