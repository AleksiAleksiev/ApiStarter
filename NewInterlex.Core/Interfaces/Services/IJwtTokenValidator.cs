namespace NewInterlex.Core.Interfaces.Services
{
    using System.Security.Claims;

    public interface IJwtTokenValidator
    {
        ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);
    }
}