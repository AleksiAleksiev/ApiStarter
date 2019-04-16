namespace NewInterlex.Infrastructure.Auth
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

    internal sealed class JwtTokenHandler : IJwtTokenHandler
    {
        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;
        private readonly ILogger<JwtTokenHandler> logger;

        internal JwtTokenHandler(JwtSecurityTokenHandler jwtSecurityTokenHandler, ILogger<JwtTokenHandler> logger)
        {
            this.jwtSecurityTokenHandler = jwtSecurityTokenHandler;
            this.logger = logger;
        }

        public string WriteToken(JwtSecurityToken jwt)
        {
            return this.jwtSecurityTokenHandler.WriteToken(jwt);
        }

        public ClaimsPrincipal ValidateToken(string token, TokenValidationParameters tokenValidationParameters)
        {
            try
            {
                var principal =
                    this.jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
            catch (Exception e)
            {
                this.logger.LogError($"Token validation failed: {e.Message}");
                return null;
            }
        }
    }
}