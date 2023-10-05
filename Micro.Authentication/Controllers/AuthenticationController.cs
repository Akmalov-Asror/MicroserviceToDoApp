using Micro.Domain.Entities;
using Micro.Domain.ExtensionFunctions;
using Micro.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Micro.Authentication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationRepository _authenticationRepository;
    public AuthenticationController(IAuthenticationRepository authenticationRepository) => _authenticationRepository = authenticationRepository;
    [HttpGet, Authorize]
    public ActionResult<string> GetMyName() => Ok(CreateTokenInJwtAuthorizationFromUsers.GetMyId());

    [HttpGet("ListUsers"), Authorize]
    public async Task<IActionResult> GetAllUsers() => Ok(await _authenticationRepository.GetAllUsers());
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(User request) => Ok(await _authenticationRepository.RegistrationAsync(request));

    [HttpPost("login")]
    public async Task<IActionResult> Login(User request) => Ok(await _authenticationRepository.Login(request));
}