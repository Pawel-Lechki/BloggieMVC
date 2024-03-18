using BloggieMVC.Data;
using BloggieMVC.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BloggieMVC.Repositories;

public class BlogPostRepository : IBlogPostRepository
{
    private readonly BloggieDbContext _dbContext;

    public BlogPostRepository(BloggieDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<BlogPost> AddAsync(BlogPost blogPost)
    {
        await _dbContext.AddAsync(blogPost);
        await _dbContext.SaveChangesAsync();
        return blogPost;
    }
    public async Task<IEnumerable<BlogPost>> GetAllAsync()
    {
        return await _dbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
    }

    public async Task<BlogPost?> GetAsync(Guid id)
    {
        return await _dbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
    {
        var existingBlogPost = await _dbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

        if (existingBlogPost != null)
        {
            existingBlogPost.Id = blogPost.Id;
            existingBlogPost.Heading = blogPost.Heading;
            existingBlogPost.PageTitle = blogPost.PageTitle;
            existingBlogPost.Content = blogPost.Content;
            existingBlogPost.ShortDescription = blogPost.ShortDescription;
            existingBlogPost.Author = blogPost.Author;
            existingBlogPost.FeaturedImageUrl = blogPost.FeaturedImageUrl;
            existingBlogPost.UrlHandle = blogPost.UrlHandle;
            existingBlogPost.Visible = blogPost.Visible;
            existingBlogPost.PublisheDate = blogPost.PublisheDate;
            existingBlogPost.Tags = blogPost.Tags;

            await _dbContext.SaveChangesAsync();
            return existingBlogPost;
        }

        return null;
    }

    public async Task<BlogPost?> DeleteAsync(Guid id)
    {
        var existingBlog = await _dbContext.BlogPosts.FindAsync(id);
        if(existingBlog != null)
        {
            _dbContext.BlogPosts.Remove(existingBlog);
            await _dbContext.SaveChangesAsync();
            return existingBlog;
        }

        return null;
    }
}