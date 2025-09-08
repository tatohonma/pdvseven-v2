using Microsoft.AspNetCore.Authorization;

namespace a7D.PDV.Ativacao.API.Middleware.Policys;

public static class AuthorizationPolicies
{


    public static readonly Dictionary<string, List<string>> Permissions = new()
    {
        
    };

    public static readonly Dictionary<string, List<string>> RolePermission = new()
    {

    };

    public static void AddPolicies(AuthorizationOptions options)
    {
        
    }
}
