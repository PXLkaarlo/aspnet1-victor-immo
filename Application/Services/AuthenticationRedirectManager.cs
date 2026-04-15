using System.Security.Claims;

namespace Application.Services;

public static class AuthenticationRedirectManager
{
    public static string? GetRedirectPath(ClaimsPrincipal claimsPrincipal, IReadOnlyDictionary<string, string> roleRedirects, string defaultPath = "/")
    {
        if (claimsPrincipal.Identity?.IsAuthenticated != true)
            return null;


        foreach (var (role, path) in roleRedirects)
        {
            if (claimsPrincipal.IsInRole(role))
                return path;
        }

        return defaultPath;
    }
}
