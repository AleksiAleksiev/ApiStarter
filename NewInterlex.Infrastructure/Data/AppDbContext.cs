using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NewInterlex.Core.Entities;
using NewInterlex.Core.Shared;

namespace NewInterlex.Infrastructure.Data
{
    using System.Threading.Tasks;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext AddAuditInfo()
        {
            var entries = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((BaseEntity)entry.Entity).Created = DateTime.UtcNow;
                }
                ((BaseEntity)entry.Entity).Modified = DateTime.UtcNow;
            }
            return this;
        }

        public async Task<int> SaveInfoAndChangesAsync()
        {
            this.AddAuditInfo();
            return await this.SaveChangesAsync();
        }
    }
}
