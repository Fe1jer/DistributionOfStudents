using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public sealed class AuthDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
            Users = Set<User>();
            UserTokens = Set<UserToken>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            modelBuilder.HasDefaultSchema("auth");

            // Addd the Postgres Extension for UUID generation
            modelBuilder.HasPostgresExtension("uuid-ossp");
        }
    }
}