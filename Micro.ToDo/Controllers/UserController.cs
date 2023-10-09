using Micro.Domain.Entities;
using Micro.ToDo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Micro.ToDo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create(User userDto)
    {
        await _userRepository.RegistrationAsync(userDto);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Login(string email)
    {
        var user = await _userRepository.GetUserByEmail(email);
        return Ok(user);
    }
}