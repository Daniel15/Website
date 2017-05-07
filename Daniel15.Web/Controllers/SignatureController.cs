using System;
using System.Linq;
using System.Numerics;
using Daniel15.Data.Repositories;
using Daniel15.Web.Extensions;
using ImageSharp;
using ImageSharp.PixelFormats;
using Microsoft.AspNetCore.Mvc;
using SixLabors.Fonts;

namespace Daniel15.Web.Controllers
{
	/// <summary>
	/// Controller to render various dynamic signature images
	/// </summary>
	[Route("sig/[action].png")]
	public partial class SignatureController : Controller
	{
		/// <summary>
		/// Padding (in pixels) to add to the right and bottom of the generated image
		/// </summary>
		private const int PADDING = 2;

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
			return RenderText("Latest blog post: " + latestPost.Title);
		}

		/// <summary>
		/// Renders the specified text to an image
		/// </summary>
		/// <param name="text">Text to render</param>
		/// <returns>PNG image</returns>
		private ActionResult RenderText(string text)
		{
			var family = FontCollection.SystemFonts.Find("Segoe UI");
			var font = new Font(family, 13);
			var measurer = new TextMeasurer();
			var size = measurer.MeasureText(text, font, 72);

			using (var image = new Image((int)Math.Ceiling(size.Width) + PADDING, (int)Math.Ceiling(size.Height) + PADDING))
			{
				image.DrawText(text, font, Rgba32.Black, new Vector2(0, 0));
				return image.ToActionResult();
			}
		}
	}
}
