namespace NewInterlex.Infrastructure.Auth
{
    using System;
    using System.Security.Cryptography;
    using Core.Interfaces.Services;

    internal sealed class TokenFactory : ITokenFactory
    {
        public string GenerateToken(int size = 32)
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}