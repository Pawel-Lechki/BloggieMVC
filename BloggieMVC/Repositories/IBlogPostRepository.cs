using System.Collections;
using BloggieMVC.Models.Domain;

namespace BloggieMVC.Repositories;

public interface IBlogPostRepository
{
    Task<IEnumerable<BlogPost>> GetAllAsync();
    Task<BlogPost?> GetAsync(Guid id);
    Task<BlogPost> AddAsync(BlogPost blogPost);
    Task<BlogPost?> UpdateAsync(BlogPost blogPost);
    Task<BlogPost?> DeleteAsync(Guid id);
}