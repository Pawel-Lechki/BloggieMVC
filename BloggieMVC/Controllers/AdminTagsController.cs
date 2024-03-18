using BloggieMVC.Data;
using BloggieMVC.Models.Domain;
using BloggieMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public async Task<IActionResult> Add(AddTagRequest addTagRequest)
    {
        var tag = new Tag
        {
            Name = addTagRequest.Name,
            DisplayName = addTagRequest.DisplayName
        };
        
        await _dbContext.Tags.AddAsync(tag);
        await _dbContext.SaveChangesAsync();
        
        return RedirectToAction("List");
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        // use dbContext to read tags
        var tags = await _dbContext.Tags.ToListAsync();
        return View(tags);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        //var tag = _dbContext.Tags.Find(id);
        var tag = await _dbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);

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
    public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
    {
        var tag = new Tag
        {
            Id = editTagRequest.Id,
            Name = editTagRequest.Name,
            DisplayName = editTagRequest.DisplayName
        };

        var existingTag = await _dbContext.Tags.FindAsync(tag.Id);
        if (existingTag != null)
        {
            existingTag.Name = tag.Name;
            existingTag.DisplayName = tag.DisplayName;

            await _dbContext.SaveChangesAsync();
            //return RedirectToAction("List");
            
            //Show success notification
            return RedirectToAction("Edit", new { id = editTagRequest.Id});
        }

        // show fail notification
        return RedirectToAction("Edit", new { id = editTagRequest.Id});
    }

    [HttpPost]
    public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
    {
        var existingTag = await _dbContext.Tags.FindAsync(editTagRequest.Id);
        if (existingTag != null)
        {
            _dbContext.Tags.Remove(existingTag);
            await _dbContext.SaveChangesAsync();
            
            // show succes notitifaction
            return RedirectToAction("List");
        }

        return RedirectToAction("Edit", new { Id = editTagRequest.Id});
    }
}