using System;
using System.Collections.Generic;
using System.Text;

namespace NewInterlex.Infrastructure.Helpers
{
    public static class Constants
    {
        // public static class Strings // if another level of distinction is needed
        public static class JwtClaimIdentifiers
        {
            public const string Rol = "rol";
            public const string Id = "id";
        }

        public static class JwtClaims
        {
            public const string User = "user";
        }
    }
}
