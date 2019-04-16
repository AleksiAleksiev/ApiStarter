using System;
using System.Collections.Generic;
using System.Text;
using NewInterlex.Core.Shared;

namespace NewInterlex.Core.Entities
{
    public class RefreshToken : BaseEntity
    {
        public RefreshToken(string token, DateTime expires, int userId)
        {
            this.Token = token ?? throw new ArgumentNullException(nameof(token));
            this.Expires = expires;
            this.UserId = userId;
        }

        public string Token { get; set; }

        public DateTime Expires { get; set; }

        public int UserId { get; set; }

        public bool Active => DateTime.UtcNow <= Expires;

    }
}
