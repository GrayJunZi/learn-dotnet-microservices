using AuthLibrary.Constants.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace AuthLibrary.Permissions;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    public PermissionAuthorizationHandler()
    {

    }
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User is null)
        {
            await Task.CompletedTask;
        }

        var permissions = context.User.Claims
            .Where(claim => claim.Type == AppClaim.Permission
                && claim.Value == requirement.Permission
                && claim.Issuer == AppClaim.Issuer);

        if (permissions.Any())
        {
            context.Succeed(requirement);
            await Task.CompletedTask;
        }
    }
}
