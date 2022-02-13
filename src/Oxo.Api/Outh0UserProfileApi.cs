using System.Net.Http.Headers;
using System.Text.Json;

namespace Oxo.Api;

public class Outh0UserProfileApi : IOuth0UserProfileApi
{
    private readonly IHttpClientFactory httpClientFactory;

    public Outh0UserProfileApi(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<UserProfile> GetUserProfileAsync(string accessToken)
    {
        var httpClient = httpClientFactory.CreateClient();

        var uri = new Uri("https://histalium.eu.auth0.com/userinfo");
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new UserProfileNamingPolicy()
        };
        var userProfile = await httpResponseMessage.Content.ReadFromJsonAsync<UserProfile>(serializerOptions);

        if (userProfile is null)
        {
            throw new Exception("User profile is null");
        }

        return userProfile;
    }
}
