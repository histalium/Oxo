using System.Text.Json;

namespace Oxo.Api;

internal class UserProfileNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name switch
        {
            "Sub" => "sub",
            "GivenName" => "given_name",
            "FamilyName" => "family_name",
            "Name" => "name",
            _ => throw new NotImplementedException(),
        };
    }
}
