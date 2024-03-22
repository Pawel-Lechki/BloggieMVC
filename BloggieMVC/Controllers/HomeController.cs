using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BloggieMVC.Models;
using BloggieMVC.Models.ViewModels;
using BloggieMVC.Repositories;

namespace BloggieMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly ITagInterface _tagRepostory;


    public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogPostRepository, ITagInterface tagRepostory)
    {
        _logger = logger;
        _blogPostRepository = blogPostRepository;
        _tagRepostory = tagRepostory;
    }

    public async Task<IActionResult> Index()
    {
        // getting all blogs
        var blogPosts = await _blogPostRepository.GetAllAsync();
        
        // get all tags
        var tags = await _tagRepostory.GetAllAsync();

        var model = new HomeViewModel
        {
            BlogPost = blogPosts,
            Tags = tags
        };
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}