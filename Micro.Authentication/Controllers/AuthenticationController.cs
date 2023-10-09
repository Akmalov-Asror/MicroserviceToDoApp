using MailKit.Net.Smtp;
using MailKit.Security;
using Micro.Domain.Data;
using Micro.Domain.Entities;
using Micro.Domain.ExtensionFunctions;
using Micro.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Timers;
using Micro.Authentication.Request;
using Micro.Authentication.Services;
using Newtonsoft.Json;
using EmailRequest = Micro.Domain.Entities.EmailRequest;
using Timer = System.Timers.Timer;

namespace Micro.Authentication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    string otherServiceUrl = "http://localhost:1005/api/User";

    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly AppDbContext _context;
    private readonly EmailProducer _emailProducer;
    private Timer timer;
    public AuthenticationController(IAuthenticationRepository authenticationRepository, AppDbContext context, EmailProducer emailProducer)
    {
        _authenticationRepository = authenticationRepository;
        _context = context;
        _emailProducer = emailProducer;
    }

    [HttpGet, Authorize]
    public ActionResult<string> GetMyName() => Ok(CreateTokenInJwtAuthorizationFromUsers.GetMyId());

    [HttpGet("ListUsers"), Authorize]
    public async Task<IActionResult> GetAllUsers() => Ok(await _authenticationRepository.GetAllUsers());

    [HttpPost("login")]
    public async Task<IActionResult> Login(User request) => Ok(await _authenticationRepository.Login(request));
    [HttpPost("register")]
    public async Task<ActionResult> Register(UserRequest request)
    {
        using (var httpClient = new HttpClient())
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(otherServiceUrl, content);
            if (response.IsSuccessStatusCode)
            {
                EmailRequest emailRequest = new EmailRequest
                {
                    ToEmail = request.Email,
                    Subject = $"Welcome to Our Website",
                    Message = $"Hello {request.FullName},\\n\\nThank you for registering on our website!"
                };
                _emailProducer.SendEmailRequest(emailRequest);
                return Ok(request);
            }
            else
            {
                return BadRequest("Something went wrong");
            }
        }
    }
}