using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewInterlex.Core.Shared;

namespace NewInterlex.Core.Entities
{
    public class User : BaseEntity
    {
        public string IdentityId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }


        public User() { }

        public User(string identityId, string userName, string email)
        {
            this.IdentityId = identityId;
            this.UserName = userName;
            this.Email = email;
        }

        public bool HasValidRefreshToken(string refreshToken)
        {
            return this.RefreshTokens.Any(rt => rt.Token == refreshToken && rt.Active);
        }

        public void AddRefreshToken(string token, int userId, double daysToExpire = 15)
        {
            var tokenToAdd = new RefreshToken(token, DateTime.UtcNow.AddDays(daysToExpire), userId);
            this.RefreshTokens.Add(tokenToAdd);
        }

        public void RemoveRefreshToken(string token)
        {
            var tokenEntity = this.RefreshTokens.First(x => x.Token == token);
            this.RefreshTokens.Remove(tokenEntity);
        }
    }
}
