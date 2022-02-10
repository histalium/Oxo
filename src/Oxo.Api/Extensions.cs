using Microsoft.OpenApi.Models;

namespace Oxo.Api;

internal static class Extensions
{
    public static OpenApiResponses AddIfNotContains(this OpenApiResponses responses, string key, string description)
    {
        if (!responses.ContainsKey(key))
        {
            responses.Add(key, new OpenApiResponse { Description = description });
        }

        return responses;
    }
}
