using BloggieMVC.Data;
using BloggieMVC.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BloggieMVC.Repositories;

public class BlogPostCommentRepository : IBlogPostCommentRepository
{
    private readonly BloggieDbContext _dbContext;

    public BlogPostCommentRepository(BloggieDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
    {
        await _dbContext.BlogPostComments.AddAsync(blogPostComment);
        await _dbContext.SaveChangesAsync();
        return blogPostComment;
    }

    public async Task<List<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId)
    {
        return await _dbContext.BlogPostComments.Where(x => x.BlogPostId == blogPostId).ToListAsync();
    }
}