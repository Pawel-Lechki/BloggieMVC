using BloggieMVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BloggieMVC.Repositories;

public class UserRepository :IUserRepository
{
    private readonly AuthDbContext _authDbContext;

    public UserRepository(AuthDbContext authDbContext)
    {
        _authDbContext = authDbContext;
    }
    public async Task<List<IdentityUser>> GetAll()
    {
        var users = await _authDbContext.Users.ToListAsync();
        var superAdminUser = await _authDbContext.Users.FirstOrDefaultAsync(x => x.Email == "superadmin@bloggie.com");

        if (superAdminUser != null)
        {
            users.Remove(superAdminUser);
        }

        return users;
    }
}