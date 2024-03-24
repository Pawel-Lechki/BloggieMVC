using BloggieMVC.Models.Domain;

namespace BloggieMVC.Repositories;

public interface IBlogPostCommentRepository
{
    Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);
    Task<List<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId);
}