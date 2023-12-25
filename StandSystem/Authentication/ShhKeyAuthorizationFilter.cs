using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StandSystem.Authentication;

public class ShhKeyAuthorizationFilter : IAuthorizationFilter
{
    private readonly ISshKeyValidator _sshKeyValidator;

    public ShhKeyAuthorizationFilter(ISshKeyValidator sshKeyValidator)
    {
        _sshKeyValidator = sshKeyValidator;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        string? key = context.HttpContext.Request.Headers["Key"];
        string ip = context.HttpContext.Connection.RemoteIpAddress.ToString();

        if (key is null || !_sshKeyValidator.IsValid(key, ip))
        {
            context.Result = new UnauthorizedResult();
        }
    }
}
