using BloggieMVC.Data;
using BloggieMVC.Models.Domain;
using BloggieMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        
        return RedirectToAction("List");
    }

    [HttpGet]
    public IActionResult List()
    {
        // use dbContext to read tags
        var tags = _dbContext.Tags.ToList();
        return View(tags);
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        //var tag = _dbContext.Tags.Find(id);
        var tag =_dbContext.Tags.FirstOrDefault(x => x.Id == id);

        if (tag != null)
        {
            var editTagRequest = new EditTagRequest
            {
                Id = tag.Id,
                Name = tag.Name,
                DisplayName = tag.DisplayName
            };
            return View(editTagRequest);    
        }
        
        return View(null);
    }

    [HttpPost]
    public IActionResult Edit(EditTagRequest editTagRequest)
    {
        var tag = new Tag
        {
            Id = editTagRequest.Id,
            Name = editTagRequest.Name,
            DisplayName = editTagRequest.DisplayName
        };

        var existingTag = _dbContext.Tags.Find(tag.Id);
        if (existingTag != null)
        {
            existingTag.Name = tag.Name;
            existingTag.DisplayName = tag.DisplayName;

            _dbContext.SaveChanges();
            //return RedirectToAction("List");
            
            //Show success notification
            return RedirectToAction("Edit", new { id = editTagRequest.Id});
        }

        // show fail notification
        return RedirectToAction("Edit", new { id = editTagRequest.Id});
    }

    [HttpPost]
    public IActionResult Delete(EditTagRequest editTagRequest)
    {
        var existingTag = _dbContext.Tags.Find(editTagRequest.Id);
        if (existingTag != null)
        {
            _dbContext.Tags.Remove(existingTag);
            _dbContext.SaveChanges();
            
            // show succes notitifaction
            return RedirectToAction("List");
        }

        return RedirectToAction("Edit", new { Id = editTagRequest.Id});
    }
}