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
        string apiKey = context.HttpContext.Request.Headers["n"];

        if (!_sshKeyValidator.IsValid(apiKey))
        {
            context.Result = new UnauthorizedResult();
        }
    }
}
