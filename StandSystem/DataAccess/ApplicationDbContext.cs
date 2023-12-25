using Microsoft.EntityFrameworkCore;
using StandSystem.IdentityScheme;

namespace StandSystem.DataAccess;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) { }

    public DbSet<Role>      Roles       { get; set; }
    public DbSet<User>      Users       { get; set; }
    public DbSet<UserRole>  UserRoles   { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.UseSerialColumns();
    }
}
