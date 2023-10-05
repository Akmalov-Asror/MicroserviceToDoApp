using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Micro.Domain.Data;
using Micro.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Micro.Domain.ServiceCollectionExtensions;

public static class ServiceCollectionExtensionsProgramCs
{
    public static void AddServiceCollection(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var build = builder.Services;
        build.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "http://localhost:5069/",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asfsafsasafjsafjksafksafsafsafsafasfasfafasfsafasfsafsafassaf"))
            };
        });

        build.AddHttpContextAccessor();
        build.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionString"));
        });
    }

}