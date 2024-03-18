using System.Collections;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BloggieMVC.Models.ViewModels;

public class AddBlogPostRequest
{
    public string Heading { get; set; }
    public string PageTitle { get; set; }
    public string Content { get; set; }
    public string ShortDescription { get; set; }
    public string FeaturedImageUrl { get; set; }
    public string UrlHandle { get; set; }
    public DateTime PublisheDate { get; set; }
    public string Author { get; set; }
    public bool Visible { get; set; }
    
    // Display tags - select one
    /*public IEnumerable<SelectListItem> Tags { get; set; }
    public string SelectedTag { get; set; }*/
    
    // Display tags - select many
    public IEnumerable<SelectListItem> Tags { get; set; }
    public string[] SelectedTags { get; set; } = Array.Empty<string>();
}