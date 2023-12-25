using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StandSystem.DataAccess;
using System.Data;
using System.Globalization;

namespace StandSystem.IdentityScheme;

public class UserStore : IUserStore<User>, IUserPasswordStore<User>, IUserRoleStore<User>
{
    private readonly ApplicationDbContext _db;
    private bool _disposed;

    public UserStore(ApplicationDbContext db)
    {
        _db = db;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _db?.Dispose();
            }

            _disposed = true;
        }
    }

    public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string?> GetUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult((string?)user.UserName);
    }

    public async Task SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
    {
        if (userName is null) 
            throw new ArgumentNullException(nameof(userName));

        await Task.Run(() => user.UserName = userName);
    }

    public Task<string?> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult((string?)user.UserName.ToUpper());
    }

    public async Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
    {
        if (normalizedName is null)
            throw new ArgumentNullException(nameof(normalizedName));

        await Task.Run(() => user.NormalizedUserName = normalizedName.ToUpper());
    }

    public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
    {
        if (user is null)
        {
            var error = new IdentityError { Code = "1", Description = "User account cannot be null" };
            return await Task.FromResult(IdentityResult.Failed(error));
        }

        user.ConcurrencyStamp = Guid.NewGuid().ToString();
        _db.Add(user);

        await _db.SaveChangesAsync(cancellationToken);

        return await Task.FromResult(IdentityResult.Success);
    }

    public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        if (user is null)
        {
            var error = new IdentityError { Code = "1", Description = "User account cannot be null" };
            return await Task.FromResult(IdentityResult.Failed(error));
        }

        user.ConcurrencyStamp = Guid.NewGuid().ToString();
        _db.Update(user);

        await _db.SaveChangesAsync(cancellationToken);

        return await Task.FromResult(IdentityResult.Success);
    }

    public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
    {
        if (user is null)
        {
            var error = new IdentityError { Code = "1", Description = "User account cannot be null" };
            return await Task.FromResult(IdentityResult.Failed(error));
        }

        _db.Remove(user);

        int i = await _db.SaveChangesAsync(cancellationToken);

        return await Task.FromResult(i == 1 ? IdentityResult.Success : IdentityResult.Failed());
    }

    public async Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _db.Users.SingleOrDefaultAsync(u => u.Id.Equals(Guid.Parse(userId)), cancellationToken);
    }

    public Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return _db.Users
            .Where(e => e.UserName.ToUpper().Equals(normalizedUserName.ToUpper()))
            .FirstOrDefaultAsync();
    }

    public Task SetPasswordHashAsync(User user, string? passwordHash, CancellationToken cancellationToken)
    {
        if (passwordHash is null) throw new ArgumentNullException(nameof(passwordHash));
        user.PasswordHash = passwordHash;

        return Task.FromResult(0);
    }

    public Task<string?> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult((string?)user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
    }

    public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user is null) throw new ArgumentNullException(nameof(user));
        if (roleName is null) throw new ArgumentNullException(nameof(roleName));

        var role = await _db.Roles.SingleOrDefaultAsync(r => r.NormalizedName == roleName.ToUpper(), cancellationToken);

        if (role is null)
        {
            throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
                "Role Not Found", roleName));
        }

        var ur = new UserRole() { UserId = user.Id, RoleId = role.Id };
        _db.UserRoles.Add(ur);
    }

    public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user is null) throw new ArgumentNullException(nameof(user));

        var role = await _db.Roles.SingleOrDefaultAsync(r => r.NormalizedName == roleName.ToUpper());

        if (role is null)
        {
            throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
                "Role Not Found", roleName));
        }

        var userRole = await _db.UserRoles.SingleOrDefaultAsync(r => r.UserId == user.Id && r.RoleId == role.Id);

        if (userRole is not null)
        {
            _db.UserRoles.Remove(userRole);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user is null) throw new ArgumentNullException(nameof(user));

        var query = from userRole in _db.UserRoles
                    where userRole.UserId.Equals(user.Id)
                    join role in _db.Roles on userRole.RoleId equals role.Id
                    select role.Name;

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user is null) throw new ArgumentNullException(nameof(user));

        var role = await _db.Roles.SingleOrDefaultAsync(r => r.NormalizedName == roleName.ToUpper(), cancellationToken);

        if (role is not null)
        {
            var userId = user.Id;
            var roleId = role.Id;
            return await _db.UserRoles.AnyAsync(ur => ur.RoleId.Equals(roleId) && ur.UserId.Equals(userId), cancellationToken);
        }

        return false;
    }

    public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (roleName is null) throw new ArgumentNullException(nameof(roleName));

        var query = from user in _db.Users
                    join role in _db.Roles on user.Id equals role.Id
                    where role.NormalizedName == roleName.ToUpper()
                    select user;

        return await query.ToListAsync(cancellationToken);
    }
}