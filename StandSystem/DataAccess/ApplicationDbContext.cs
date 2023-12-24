using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StandSystem.IdentityScheme;
using StandSystem.Models;

namespace StandSystem.DataAccess;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    //public DbSet<Student> Students { get; set; }

    public DbSet<Role>      Roles       {  get; set; }
    public DbSet<User>      Users       { get; set; }
    public DbSet<UserRole>  UserRoles   { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /*builder.Entity<IdentityUser>(entity =>
        {
            entity.ToTable(name: "Users");
        });

        builder.Entity<IdentityUser>().Ignore(u => u.Email)
                                      .Ignore(u => u.NormalizedEmail)
                                      .Ignore(u => u.EmailConfirmed)
                                      .Ignore(u => u.PhoneNumber)
                                      .Ignore(u => u.PhoneNumberConfirmed)
                                      .Ignore(u => u.TwoFactorEnabled);

        builder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable(name: "Roles");
        });

        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable(name: "UserRoles");
        });

        builder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable(name: "UserClaims");
        });

        builder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable(name: "UserLogins");
        });

        builder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable(name: "RoleClaims");
        });

        builder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable(name: "UserTokens");
        });*/

        builder.UseSerialColumns();
    }
}
