using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StandSystem.DataAccess;

namespace StandSystem.IdentityScheme;

public class RoleStore : IRoleStore<Role>, IQueryableRoleStore<Role>
{
    private readonly ApplicationDbContext _db;
    private bool _disposed;

    public RoleStore(ApplicationDbContext db)
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

    public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (role is null)
        {
            var error = new IdentityError { Code = "1", Description = "Role cannot be null" };
            return await Task.FromResult(IdentityResult.Failed(error));
        }

        role.ConcurrencyStamp = Guid.NewGuid().ToString();
        _db.Roles.Add(role);
        await _db.SaveChangesAsync(cancellationToken);

        return await Task.FromResult(IdentityResult.Success);
    }

    public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (role is null)
        {
            var error = new IdentityError { Code = "1", Description = "Role cannot be null" };
            return await Task.FromResult(IdentityResult.Failed(error));
        }

        _db.Roles.Attach(role);
        role.ConcurrencyStamp = Guid.NewGuid().ToString();
        _db.Roles.Update(role);

        try
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return await Task.FromResult(IdentityResult.Failed());
        }

        return await Task.FromResult(IdentityResult.Success);
    }

    public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (role is null)
        {
            var error = new IdentityError { Code = "1", Description = "Role cannot be null" };
            return await Task.FromResult(IdentityResult.Failed(error));
        }

        _db.Roles.Remove(role);

        try
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return await Task.FromResult(IdentityResult.Failed());
        }

        return await Task.FromResult(IdentityResult.Success);
    }

    public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (role is null)
        {
            throw new ArgumentNullException(nameof(role));
        }

        return Task.FromResult(role.Id.ToString());
    }

    public Task<string?> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (role is null)
        {
            throw new ArgumentNullException(nameof(role));
        }

        return Task.FromResult((string?)role.Name);
    }

    public Task SetRoleNameAsync(Role role, string? roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (roleName is null) throw new ArgumentNullException(nameof(roleName));

        if (role is null)
        {
            throw new ArgumentNullException(nameof(role));
        }

        role.Name = roleName;

        return Task.CompletedTask;
    }

    public Task<string?> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (role is null)
        {
            throw new ArgumentNullException(nameof(role));
        }

        return Task.FromResult((string?)role.NormalizedName);
    }

    public Task SetNormalizedRoleNameAsync(Role role, string? normalizedName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (normalizedName is null) throw new ArgumentNullException(nameof(normalizedName));

        if (role is null)
        {
            throw new ArgumentNullException(nameof(role));
        }

        role.NormalizedName = normalizedName.ToUpper();
        return Task.CompletedTask;
    }

    Task<Role?> IRoleStore<Role>.FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return _db.Roles.FirstOrDefaultAsync(u => u.Id.Equals(roleId), cancellationToken);
    }

    Task<Role?> IRoleStore<Role>.FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return _db.Roles.FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, cancellationToken);
    }

    public IQueryable<Role> Roles => _db.Roles.AsQueryable();
}
