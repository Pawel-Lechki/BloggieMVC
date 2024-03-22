using BloggieMVC.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BloggieMVC.Controllers;

public class BlogsController : Controller
{
    private readonly IBlogPostRepository _blogPostRepository;

    public BlogsController(IBlogPostRepository blogPostRepository)
    {
        _blogPostRepository = blogPostRepository;
    }
    [HttpGet]
    public async Task<IActionResult> Index(string urlHandle)
    {
        var blogPost = await _blogPostRepository.GetByUrlHandelAsync(urlHandle);
        return View(blogPost);
    }
}