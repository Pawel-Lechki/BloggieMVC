using BloggieMVC.Data;
using BloggieMVC.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BloggieMVC.Repositories;

public class BlogPostLikeRepository : IBlogPostLikeRepository
{
    private readonly BloggieDbContext _dbContext;

    public BlogPostLikeRepository(BloggieDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<int> GetTotalLikes(Guid blogPostId)
    {
        return await _dbContext.BlogPostLikes.CountAsync(x => x.BlogPostId == blogPostId);
    }

    public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
    {
        return await _dbContext.BlogPostLikes.Where(x => x.BlogPostId == blogPostId).ToListAsync();
    }

    public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
    {
        await _dbContext.BlogPostLikes.AddAsync(blogPostLike);
        await _dbContext.SaveChangesAsync();
        return blogPostLike;
    }
}