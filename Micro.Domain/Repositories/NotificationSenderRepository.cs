using MailKit.Net.Smtp;
using MailKit.Security;
using Micro.Domain.Entities;
using Micro.Domain.ServiceCollectionExtensions;
using MimeKit.Text; 
using MimeKit;
using static System.Net.WebRequestMethods;

namespace Micro.Domain.Repositories;
public class NotificationSenderRepository : INotificationSenderRepository
{
    public async Task ToDoSender(EmailSender emailSender, string message)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("akmalovasror0@gmail.com"));
        email.To.Add(MailboxAddress.Parse(emailSender.Email));
        email.Subject = "Your verification code";
        email.Body = new TextPart(TextFormat.Html) { Text =  message }; 

        var smpt = new SmtpClient();
        await smpt.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await smpt.AuthenticateAsync("akmalovasror0@gmail.com", "ezebzxgragnvnlco");
        await smpt.SendAsync(email);
        await smpt.DisconnectAsync(true);

    }
}