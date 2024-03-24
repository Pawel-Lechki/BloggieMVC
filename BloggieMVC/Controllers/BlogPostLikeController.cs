using BloggieMVC.Models.Domain;
using BloggieMVC.Models.ViewModels;
using BloggieMVC.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BloggieMVC.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogPostLikeController : ControllerBase
{
   private readonly IBlogPostLikeRepository _blogPostLikeRepository;

   public BlogPostLikeController(IBlogPostLikeRepository blogPostLikeRepository)
   {
      _blogPostLikeRepository = blogPostLikeRepository;
   }
   
   [HttpPost]
   [Route("Add")]
   public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest)
   {
      var model = new BlogPostLike
      {
         Id = addLikeRequest.BlogPostId,
         UserId = addLikeRequest.UserId
      };
      await _blogPostLikeRepository.AddLikeForBlog(model);

      return Ok();
   }

   [HttpGet]
   [Route("{blogPostId:guid}/totalLikes")]
   public async Task<IActionResult> GetTotalLikesForBlog([FromRoute] Guid blogPostId)
   {
      var totalLikes = await _blogPostLikeRepository.GetTotalLikes(blogPostId);

      return Ok(totalLikes);
   }
}