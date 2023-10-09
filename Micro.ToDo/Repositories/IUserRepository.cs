using Micro.Domain.Entities;

namespace Micro.ToDo.Repositories;

public interface IUserRepository
{
    System.Threading.Tasks.Task RegistrationAsync(User user);
    Task<User> GetUserByEmail(string email);
}