using BloggieMVC.Data;
using BloggieMVC.Models.Domain;
using BloggieMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BloggieMVC.Controllers;

public class AdminTagsController : Controller
{
    private readonly BloggieDbContext _dbContext;

    public AdminTagsController(BloggieDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }
    [HttpPost]
    [ActionName("Add")]
    public IActionResult SubmitTag(AddTagRequest addTagRequest)
    {
        var tag = new Tag
        {
            Name = addTagRequest.Name,
            DisplayName = addTagRequest.DisplayName
        };
        
        _dbContext.Tags.Add(tag);
        _dbContext.SaveChanges();
        
        return View("Add");
    }
}