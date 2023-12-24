using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StandSystem.DataAccess;

namespace StandSystem.IdentityScheme;

public class UserStore : IUserStore<User>, IUserPasswordStore<User>
{
    private readonly ApplicationDbContext _db;

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
        if (disposing)
        {
            _db?.Dispose();
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
        if (userName == null) throw new ArgumentNullException(nameof(userName));
        await Task.Run(() => user.UserName = userName);

        //user.UserName = userName;
        //_db.Update(user);
        //await _db.SaveChangesAsync(cancellationToken);
    }

    public Task<string?> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult((string?)user.UserName.ToUpper());
    }

    public async Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
    {
        if (normalizedName == null) throw new ArgumentNullException(nameof(normalizedName));
        await Task.Run(() => user.NormalizedUserName = normalizedName.ToUpper());

        //user.NormalizedUserName = normalizedName;
        //_db.Update(user);
        //await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
    {
        if (user == null)
        {
            var error = new IdentityError { Code = "1", Description = "User account cannot be null" };
            return await Task.FromResult(IdentityResult.Failed(error));
        }

        _db.Add(user);

        await _db.SaveChangesAsync(cancellationToken);

        return await Task.FromResult(IdentityResult.Success);
    }

    public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        if (user == null)
        {
            var error = new IdentityError { Code = "1", Description = "User account cannot be null" };
            return await Task.FromResult(IdentityResult.Failed(error));
        }

        _db.Update(user);

        await _db.SaveChangesAsync(cancellationToken);

        return await Task.FromResult(IdentityResult.Success);
    }

    public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
    {
        if (user == null)
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
        if (int.TryParse(userId, out int id))
        {
            return await _db.Users.FindAsync(id);
        }
        else
        {
            return await Task.FromResult((User?)null);
        }
    }

    public Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return _db.Users
            .Where(e => e.UserName.ToUpper().Equals(normalizedUserName.ToUpper()))
            .FirstOrDefaultAsync();
    }

    public Task SetPasswordHashAsync(User user, string? passwordHash, CancellationToken cancellationToken)
    {
        if (passwordHash == null) throw new ArgumentNullException(nameof(passwordHash));
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
}