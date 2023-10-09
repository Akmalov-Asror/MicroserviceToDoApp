using Micro.Domain.Data;
using Micro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BCryptNet = BCrypt.Net;

namespace Micro.ToDo.Repositories.Implementation;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) => _context = context;

    public async System.Threading.Tasks.Task RegistrationAsync(User user)
    {
        var salt = BCryptNet.BCrypt.GenerateSalt();
        var passwordHash
            = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);

        user.Password = passwordHash;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email)!;
    }

    public async Task<string> GetUserById(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        return user!.Email!;
    }
}