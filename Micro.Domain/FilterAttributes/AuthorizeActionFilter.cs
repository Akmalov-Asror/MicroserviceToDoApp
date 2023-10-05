using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Micro.Domain.FilterAttributes;
public class AuthorizeActionFilter : IAuthorizationFilter
{
    private readonly string _permission;
    public AuthorizeActionFilter(string permission) => _permission = permission;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var isAuthorized = CheckUserPermission(context.HttpContext.User, _permission);
        if (!isAuthorized) context.Result = new UnauthorizedResult();
    }
    public bool CheckUserPermission(ClaimsPrincipal user, string permission) => permission == "Read";
}