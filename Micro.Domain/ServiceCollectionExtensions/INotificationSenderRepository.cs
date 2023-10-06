using Micro.Domain.Entities;

namespace Micro.Domain.ServiceCollectionExtensions;

public interface INotificationSenderRepository
{
    Task ToDoSender(EmailSender emailSender, string message);
}