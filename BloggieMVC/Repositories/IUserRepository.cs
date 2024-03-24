using Microsoft.AspNetCore.Identity;

namespace BloggieMVC.Repositories;

public interface IUserRepository
{
    Task<List<IdentityUser>> GetAll();
}