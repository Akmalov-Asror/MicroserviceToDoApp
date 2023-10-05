using Micro.Domain.Entities;

namespace Micro.Domain.Interfaces;

public interface IAuthenticationRepository
{
    Task<User> RegistrationAsync(User user);
    Task<string> Login(User user);
    Task<List<User>> GetAllUsers();
}