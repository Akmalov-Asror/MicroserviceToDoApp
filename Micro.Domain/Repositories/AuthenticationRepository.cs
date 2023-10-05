using Micro.Domain.Data;
using Micro.Domain.Entities;
using Micro.Domain.ExtensionFunctions;
using Micro.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using BCryptNet = BCrypt.Net;

namespace Micro.Domain.Repositories;
public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly AppDbContext _context;

    public AuthenticationRepository(AppDbContext context) => _context = context;

    public async Task<User> RegistrationAsync(User request)
    {
        var salt = BCryptNet.BCrypt.GenerateSalt();
        var passwordHash
            = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = passwordHash
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<string> Login(User request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == request.Email);

        if (user != null)
        {
            var verify = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (verify)
            {
                var token = CreateTokenInJwtAuthorizationFromUsers.CreateToken(user);
                return token;
            }
            else throw new BadHttpRequestException("Wrong password");
        }

        throw new BadHttpRequestException("User not found.");
    }

    public async Task<List<User>> GetAllUsers() => await _context.Users.ToListAsync();
}