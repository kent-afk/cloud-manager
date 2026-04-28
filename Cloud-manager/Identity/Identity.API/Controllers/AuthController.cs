using System.Runtime.InteropServices.ComTypes;
using Identity.Application.UseCases.EmailChange;
using Identity.Application.UseCases.LoginUser;
using Identity.Application.UseCases.PasswordChange;
using Identity.Application.UseCases.UserCreated;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, 
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.Succeeded)
            return Ok(result.Value);
        
        return BadRequest(result.ErrorMessage);
    }
    
    [HttpPost("login")] 
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.Succeeded)
            return Ok(result.Value);
        
        return BadRequest(result.ErrorMessage);
    }

    [Authorize]
    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.Succeeded)
            return Ok(result.Value);
        
        return BadRequest(result.ErrorMessage);
    }
    
    [Authorize]
    [HttpPut("email")]
    public async Task<IActionResult> ChangeEmail([FromBody] EmailChangeCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.Succeeded)
            return Ok(result.Value);
        
        return BadRequest(result.ErrorMessage);
    }
}