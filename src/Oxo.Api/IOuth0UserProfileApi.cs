namespace Oxo.Api;

public interface IOuth0UserProfileApi
{
    Task<UserProfile> GetUserProfileAsync(string accessToken);
}
