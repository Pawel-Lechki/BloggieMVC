using BloggieMVC.Data;
using BloggieMVC.Models.Domain;
using BloggieMVC.Models.ViewModels;
using BloggieMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BloggieMVC.Controllers;

[Authorize(Roles ="Admin")]
public class AdminTagsController : Controller
{
    private readonly ITagInterface _tagRepository;

    public AdminTagsController(ITagInterface tagRepository)
    {
        _tagRepository = tagRepository;
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

        await _tagRepository.AddAsync(tag);
        
        return RedirectToAction("List");
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        // use dbContext to read tags
        var tags = await _tagRepository.GetAllAsync();
        return View(tags);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var tag = await _tagRepository.GetAsync(id);

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

        var updatedTag = await _tagRepository.UpdateAsync(tag);
        if (updatedTag != null)
        {
            // show success notification
        }
        else
        {
            // show error notification
        }

        // show fail notification
        return RedirectToAction("Edit", new { id = editTagRequest.Id});
    }

    [HttpPost]
    public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
    {
        var deleteTag = await _tagRepository.DeleteAsync(editTagRequest.Id);
        if (deleteTag != null)
        {
            // Show success notitifaction
            return RedirectToAction("List");
        }
        
        // show error notitifaction
        return RedirectToAction("Edit", new { Id = editTagRequest.Id});
    }
}