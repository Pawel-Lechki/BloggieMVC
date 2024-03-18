using BloggieMVC.Models.Domain;
using BloggieMVC.Models.ViewModels;
using BloggieMVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BloggieMVC.Controllers;

public class AdminBlogPostController : Controller
{
    private readonly ITagInterface _tagRepository;
    private readonly IBlogPostRepository _blogPostRepository;

    public AdminBlogPostController(ITagInterface tagRepository, IBlogPostRepository blogPostRepository)
    {
        _tagRepository = tagRepository;
        _blogPostRepository = blogPostRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> Add()
    {
        // get tags form repository
        var tags = await _tagRepository.GetAllAsync();

        var model = new AddBlogPostRequest
        {
            Tags = tags.Select(x => new SelectListItem { Text = x.DisplayName, Value = x.Id.ToString() })
        };
        
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
    {
        // map view model to domain model
        var blogPost = new BlogPost
        {
            Heading = addBlogPostRequest.Heading,
            PageTitle = addBlogPostRequest.PageTitle,
            Content = addBlogPostRequest.Content,
            ShortDescription = addBlogPostRequest.ShortDescription,
            FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
            UrlHandle = addBlogPostRequest.UrlHandle,
            PublisheDate = addBlogPostRequest.PublisheDate,
            Author = addBlogPostRequest.Author,
            Visible = addBlogPostRequest.Visible,
        };
        
        // Map tags from selected tags
        var selectedTags = new List<Tag>();
        foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
        {
            var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
            var existingTag = await _tagRepository.GetAsync(selectedTagIdAsGuid);

            if (existingTag != null)
            {
                selectedTags.Add(existingTag);
            }
        }
        
        // mapping tags to domain model
        blogPost.Tags = selectedTags;
        
        await _blogPostRepository.AddAsync(blogPost);
            
        return RedirectToAction("Add");
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        // Call Repository to get data
        var blogPosts = await _blogPostRepository.GetAllAsync();
        return View(blogPosts);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        // Retrive teh result form the repository
        var blogPost = await _blogPostRepository.GetAsync(id);
        var tagsDomainModel = await _tagRepository.GetAllAsync();

        if (blogPost != null)
        {
            // Map domain model to view model
            var model = new EditBlogPostRequest
            {
                Id = blogPost.Id,
                Heading = blogPost.Heading,
                PageTitle = blogPost.PageTitle,
                Content = blogPost.Content,
                Author = blogPost.Author,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                ShortDescription = blogPost.ShortDescription,
                PublisheDate = blogPost.PublisheDate,
                Visible = blogPost.Visible,
                Tags = tagsDomainModel.Select(x => new SelectListItem
                {
                    Text = x.Name, Value = x.Id.ToString()
                }),
                SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
            };
            
            // Pass data to view
            return View(model);
        }

        
        return View(null);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
    {
        // Map view model back to domain model
        var blogPostDomainModel = new BlogPost
        {
            Id = editBlogPostRequest.Id,
            Heading = editBlogPostRequest.Heading,
            PageTitle = editBlogPostRequest.PageTitle,
            Content = editBlogPostRequest.Content,
            Author = editBlogPostRequest.Author,
            ShortDescription = editBlogPostRequest.ShortDescription,
            FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
            PublisheDate = editBlogPostRequest.PublisheDate,
            UrlHandle = editBlogPostRequest.UrlHandle,
            Visible = editBlogPostRequest.Visible
        };
        
        // Map tags into domain model
        var selectedTags = new List<Tag>();
        foreach (var selectedTag in editBlogPostRequest.SelectedTags)
        {
            if (Guid.TryParse(selectedTag, out var tag))
            {
                var foundTag = await _tagRepository.GetAsync(tag);
                if (foundTag != null)
                {
                    selectedTags.Add(foundTag);
                }
            }
        }

        blogPostDomainModel.Tags = selectedTags;

        // Submit information to repository to update
        var updatedBlog = await _blogPostRepository.UpdateAsync(blogPostDomainModel);

        if (updatedBlog != null)
        {
            // Show success notifiaction
            return RedirectToAction("Edit");
        }
        
        // Show error notification
        return RedirectToAction("Edit");
        
    }

    [HttpPost]
    public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
    {
        // Talk to repository to delete this blog post and tags
        var deletedBlogPost = await _blogPostRepository.DeleteAsync(editBlogPostRequest.Id);
        if (deletedBlogPost != null)
        {
            // Show success notification
            return RedirectToAction("List");
        }

        // Show error notitification
        return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
    }
}