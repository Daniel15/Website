using Daniel15.Web.Services;
using Daniel15.Data;
using Daniel15.Data.Entities.Blog;
using Daniel15.Data.Repositories;
using Daniel15.Web.Areas.Api.Models.Blog;
using Daniel15.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Daniel15.Web.Areas.Api.Controllers
{
	/// <summary>
	/// API for blog posts
	/// </summary>
	[Area("Api")]
	[Route("api/posts/{postId}/[action]")]
	public partial class PostsApiController : Controller
	{
		private readonly IBlogRepository _blogRepository;
		private readonly IUrlShortener _urlShortener;

		/// <summary>
		/// Initializes a new instance of the <see cref="PostsApiController" /> class.
		/// </summary>
		/// <param name="blogRepository">The blog repository.</param>
		/// <param name="urlShortener">The URL shortener.</param>
		public PostsApiController(IBlogRepository blogRepository, IUrlShortener urlShortener)
		{
			_blogRepository = blogRepository;
			_urlShortener = urlShortener;
		}

		/// <summary>
		/// Gets the URL to the specified blog post
		/// </summary>
		/// <param name="postId">ID of the post to get the URL for</param>
		/// <returns>URL to the post</returns>
		[ActionName("Url")]
		[HttpGet]
		public IActionResult Urls(int postId)
		{
			PostModel post;
			try
			{
				post = _blogRepository.Get(postId);
			}
			catch (EntityNotFoundException)
			{
				// Throw a 404 if the post doesn't exist
				return NotFound();
			}

			return new ObjectResult(new PostUrlsModel
			{
				Url = Url.BlogPostAbsolute(post),
				ShortUrl = Url.Action("Blog", "ShortUrl", new { alias = _urlShortener.Shorten(post) }, Request.Scheme),
			});
		}
	}
}
