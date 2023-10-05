using System.IdentityModel.Tokens.Jwt;
using Micro.Domain.Entities;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Micro.Domain.ExtensionFunctions;

public class CreateTokenInJwtAuthorizationFromUsers
{
    private static readonly IHttpContextAccessor? HttpContextAccessor;
    public static string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asfsafsasafjsafjksafksafsafsafsafasfasfafasfsafasfsafsafassaf"));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(7),
            signingCredentials: creds,
            issuer: "http://localhost:5069/"
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
    public static string GetMyId()
    {
        var result = string.Empty;
        if (HttpContextAccessor?.HttpContext is not null)
        {
            result = HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        return result;
    }
}