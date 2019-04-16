namespace NewInterlex.Infrastructure.Interfaces
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using Microsoft.IdentityModel.Tokens;

    public interface IJwtTokenHandler
    {
        string WriteToken(JwtSecurityToken jwt);
        ClaimsPrincipal ValidateToken(string token, TokenValidationParameters tokenValidationParameters);
    }
}