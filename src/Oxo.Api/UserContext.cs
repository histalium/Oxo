using System.Security.Claims;

namespace Oxo.Api;

internal class UserContext
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly GetUserForSubject getUserForSubject;
    private User? user;

    public UserContext(
        IHttpContextAccessor httpContextAccessor,
        GetUserForSubject getUserForSubject)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.getUserForSubject = getUserForSubject;
    }

    public async Task<User?> GetUserAsync()
    {
        if (user is not null)
        {
            return user;
        }

        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext is null)
        {
            return null;
        }

        var subject = httpContext.User.Claims
            .Where(t => t.Type == ClaimTypes.NameIdentifier)
            .FirstOrDefault()?.Value;

        if (string.IsNullOrEmpty(subject))
        {
            return null;
        }

        user = await getUserForSubject.GetUserAsync(subject);
        return user;
    }
}
