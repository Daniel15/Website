using System.Linq;
using Daniel15.Data.Repositories;
using Daniel15.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Daniel15.Web.Controllers
{
	/// <summary>
	/// Controller to render various dynamic signature images
	/// </summary>
	[Route("sig/[action].png")]
	public partial class SignatureController : Controller
	{
		private readonly IBlogRepository _blogRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="SignatureController" /> class.
		/// </summary>
		/// <param name="blogRepository">The blog repository.</param>
		public SignatureController(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}

		/// <summary>
		/// Generates a signature image displaying the latest blog post
		/// </summary>
		/// <returns></returns>
		[ResponseCache(Duration = 60 * 5, Location = ResponseCacheLocation.Any)]
		public virtual ActionResult LatestBlogPost()
		{
			var latestPost = _blogRepository.LatestPosts(1).First();
			var text = "Latest blog post: " + latestPost.Title;
			return File(TextRenderer.RenderTextToBytes(text), "image/png");	
		}
	}
}
