namespace NewInterlex.Infrastructure.Auth
{
    using System.Security.Claims;
    using System.Text;
    using Core.Interfaces.Services;
    using Interfaces;
    using Microsoft.IdentityModel.Tokens;

    internal class JwtTokenValidator : IJwtTokenValidator
    {
        private readonly IJwtTokenHandler jwtTokenHandler;

        internal JwtTokenValidator(IJwtTokenHandler jwtTokenHandler)
        {
            this.jwtTokenHandler = jwtTokenHandler;
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                ValidateLifetime = false // we check expired tokens here, o, rly???
            };
            return this.jwtTokenHandler.ValidateToken(token, tokenValidationParameters);
        }
    }
}