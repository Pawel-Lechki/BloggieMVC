using BloggieMVC.Models.Domain;

namespace BloggieMVC.Models.ViewModels;

public class HomeViewModel
{
    public List<BlogPost> BlogPost { get; set; }
    public List<Tag> Tags { get; set; }
}