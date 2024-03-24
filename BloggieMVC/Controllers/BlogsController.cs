using BloggieMVC.Models.Domain;
using BloggieMVC.Models.ViewModels;
using BloggieMVC.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BloggieMVC.Controllers;

public class BlogsController : Controller
{
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly IBlogPostLikeRepository _blogPostLikeRepository;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IBlogPostCommentRepository _blogPostCommentRepository;

    public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogPostLikeRepository, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IBlogPostCommentRepository blogPostCommentRepository)
    {
        _blogPostRepository = blogPostRepository;
        _blogPostLikeRepository = blogPostLikeRepository;
        _signInManager = signInManager;
        _userManager = userManager;
        _blogPostCommentRepository = blogPostCommentRepository;
    }
    [HttpGet]
    public async Task<IActionResult> Index(string urlHandle)
    {
        var liked = false;
        var blogPost = await _blogPostRepository.GetByUrlHandelAsync(urlHandle);
        var blogDerailsViewModel = new BlogDetailsViewModel();
        
        if (blogPost != null)
        {
            var totalLikes = await _blogPostLikeRepository.GetTotalLikes(blogPost.Id);

            if (_signInManager.IsSignedIn(User))
            {
                // Get Like for this blog for this user
                var likesForBlog = await _blogPostLikeRepository.GetLikesForBlog(blogPost.Id);
                
                var userId = _userManager.GetUserId(User);
                if (userId != null)
                {
                    var likeFromUser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                    liked = likeFromUser != null;
                }
            }
            
            // Get comments for blog post
            var blogCommenstsDomainModel = await _blogPostCommentRepository.GetCommentsByBlogIdAsync(blogPost.Id);

            var blogCommentForView = new List<BlogComment>();

            foreach (var blogComment in blogCommenstsDomainModel)
            {
                blogCommentForView.Add(new BlogComment
                {
                    Description = blogComment.Description,
                    DateAdded = blogComment.DateAdded,
                    Username = (await _userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName
                });
            }
            
            blogDerailsViewModel = new BlogDetailsViewModel
            {
                Id = blogPost.Id,
                Content = blogPost.Content,
                PageTitle = blogPost.PageTitle,
                Author = blogPost.Author,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Heading = blogPost.Heading,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = blogPost.UrlHandle,
                Visible = blogPost.Visible,
                Tags = blogPost.Tags,
                TotalLikes = totalLikes,
                Liked = liked,
                Comments = blogCommentForView
            };
        }
        
        return View(blogDerailsViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
    {
        if (_signInManager.IsSignedIn(User))
        {
            var domainModel = new BlogPostComment
            {
                BlogPostId = blogDetailsViewModel.Id,
                Description = blogDetailsViewModel.CommentDescription,
                UserId = Guid.Parse(_userManager.GetUserId(User)),
                DateAdded = DateTime.Now
            };

            await _blogPostCommentRepository.AddAsync(domainModel);
            return RedirectToAction("Index", "Blogs", new { urlHandle = blogDetailsViewModel.UrlHandle});
        }

        return View();
    }
}