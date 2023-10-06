using Micro.Domain.Data;
using Micro.Domain.Entities;
using Micro.Domain.ServiceCollectionExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Micro.NotificationSender.Controller;

[Route("api/[controller]")]
[ApiController]
public class EmailSenderController : ControllerBase
{
    private readonly INotificationSenderRepository _notificationSenderRepository;
    public EmailSenderController(INotificationSenderRepository notificationSenderRepository) => _notificationSenderRepository = notificationSenderRepository;

    [HttpPost]
    public async Task<IActionResult> SendEmailMessage(EmailSender emailSender, string message)
    {
         await _notificationSenderRepository.ToDoSender(emailSender, message);
         return Ok();
    }
}