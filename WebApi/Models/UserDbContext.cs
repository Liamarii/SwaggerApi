using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    internal sealed class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User>? Users { get; set; }
    }
}
