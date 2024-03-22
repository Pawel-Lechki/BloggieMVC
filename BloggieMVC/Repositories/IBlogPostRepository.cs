using System.Collections;
using BloggieMVC.Models.Domain;

namespace BloggieMVC.Repositories;

public interface IBlogPostRepository
{
    Task<List<BlogPost>> GetAllAsync();
    Task<BlogPost?> GetAsync(Guid id);
    Task<BlogPost?> GetByUrlHandelAsync(string urlHandel);
    Task<BlogPost> AddAsync(BlogPost blogPost);
    Task<BlogPost?> UpdateAsync(BlogPost blogPost);
    Task<BlogPost?> DeleteAsync(Guid id);
}