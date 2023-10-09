using MailKit.Net.Smtp;
using MailKit.Security;
using Micro.Domain.Data;
using Micro.Domain.Entities;
using Micro.Domain.ServiceCollectionExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Text;
using MimeKit;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Micro.NotificationSender.Controller;

[Route("api/[controller]")]
[ApiController]
public class EmailSenderController : ControllerBase
{
    private readonly AppDbContext _context;
    /* private readonly INotificationSenderRepository _notificationSenderRepository;
     public EmailSenderController(INotificationSenderRepository notificationSenderRepository) => _notificationSenderRepository = notificationSenderRepository;

     [HttpPost]
     public async Task<IActionResult> SendEmailMessage(EmailSender emailSender, string message)
     {
          await _notificationSenderRepository.ToDoSender(emailSender, message);
          return Ok();
     }*/

    private Timer timer;

    public EmailSenderController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult SendMessages()
    {
        try
        {
            // Timer yaratish va sozlash (5000ms = 5 sekund)
            timer = new Timer(5000);
            timer.Elapsed += async (sender, e) => await SendEmails();
            timer.AutoReset = true;
            timer.Enabled = true;

            return Ok("Xabarlarni yuborish boshlandi.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Server xatosi: {ex.Message}");
        }
    }

    private async System.Threading.Tasks.Task SendEmails()
    {
        var emailSender = new EmailSender();
        var users = await _context.Users.ToListAsync();

        foreach (var user in users)
        {
            var message = "Qalesan"; // Xabarni o'zgartiring
            await ToDoSender(emailSender, user.Email, message);
        }
    }

    private async System.Threading.Tasks.Task ToDoSender(EmailSender emailSender, string email, string message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(MailboxAddress.Parse("akmalovasror0@gmail.com"));
        emailMessage.To.Add(MailboxAddress.Parse(email));
        emailMessage.Subject = "Your verification code";
        emailMessage.Body = new TextPart(TextFormat.Html) { Text = message };

        var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync("akmalovasror0@gmail.com", "ezebzxgragnvnlco");
        await smtp.SendAsync(emailMessage);
        await smtp.DisconnectAsync(true);
    }
}