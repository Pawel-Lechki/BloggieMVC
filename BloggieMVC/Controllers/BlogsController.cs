using Microsoft.AspNetCore.Mvc;

namespace BloggieMVC.Controllers;

public class BlogsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}