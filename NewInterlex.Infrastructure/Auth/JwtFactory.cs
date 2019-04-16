namespace NewInterlex.Infrastructure.Auth
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using Core.Dto;
    using Core.Interfaces.Services;
    using Interfaces;
    using Microsoft.Extensions.Options;

    internal sealed class JwtFactory : IJwtFactory
    {
        private readonly IJwtTokenHandler jwtTokenHandler;
        private readonly JwtOptions jwtOptions;

        internal JwtFactory(IJwtTokenHandler jwtTokenHandler, IOptions<JwtOptions> jwtOptions)
        {
            this.jwtTokenHandler = jwtTokenHandler;
            this.jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(this.jwtOptions);
        }

        public async Task<AccessToken> GenerateEncodedToken(string id, string userName)
        {
            var identity = GenerateClaimsIdentity(id, userName);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, await this.jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(this.jwtOptions.IssuedAt).ToString(),
                    ClaimValueTypes.Integer64),
                identity.FindFirst(Helpers.Constants.JwtClaimIdentifiers.Rol),
                identity.FindFirst(Helpers.Constants.JwtClaimIdentifiers.Id)
            };

            var jwt = new JwtSecurityToken(this.jwtOptions.Issuer, this.jwtOptions.Audience, claims,
                this.jwtOptions.NotBefore, this.jwtOptions.Expiration, this.jwtOptions.SigningCredentials);
            
            return new AccessToken(this.jwtTokenHandler.WriteToken(jwt), (int)this.jwtOptions.ValidFor.TotalSeconds);
        }

        private static long ToUnixEpochDate(DateTime date)
        {
            return (long) Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);
        }

        private static ClaimsIdentity GenerateClaimsIdentity(string id, string userName)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                new Claim(Helpers.Constants.JwtClaimIdentifiers.Id, id),
//                new Claim(Helpers.Constants.JwtClaimIdentifiers.Rol, ), // add here new claims 
            });
        }

        private static void ThrowIfInvalidOptions(JwtOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtOptions.JtiGenerator));
            }
        }
    }
}