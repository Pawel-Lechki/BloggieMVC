using System.Net;
using BloggieMVC.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BloggieMVC.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly IImageRepository _imageRepository;

    public ImagesController(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    [HttpPost]
    public async Task<IActionResult> UploadAsync(IFormFile file)
    {
        // call a repository
        var imagegURL = await _imageRepository.UploadAsync(file);

        if (imagegURL == null)
        {
            return Problem("Something went wrong!", null, (int)HttpStatusCode.InternalServerError);
        }

        return new JsonResult(new { link = imagegURL });
    }
}