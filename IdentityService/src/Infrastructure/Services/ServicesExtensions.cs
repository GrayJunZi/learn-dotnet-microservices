using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public static class ServicesExtensions
{
    public static List<string> GetIdentityResultErrorDescriptions(this IdentityResult identityResult)
    {
        var errorDescriptions = new List<string>();
        foreach (var error in identityResult.Errors)
        {
            errorDescriptions.Add(error.Description);
        }
        return errorDescriptions;
    }
}
