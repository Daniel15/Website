using System;
using System.Linq;
using System.Numerics;
using Daniel15.Web.Repositories;
using Daniel15.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

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
			var family = SystemFonts.Get("Segoe UI");
			var font = new Font(family, 13);
			var size = TextMeasurer.MeasureSize(text, new TextOptions(font));

			using var image = new Image<Rgba32>((int)Math.Ceiling(size.Width) + PADDING, (int)Math.Ceiling(size.Height) + PADDING);
			image.Mutate(x => x.DrawText(text, font, Color.Black, new Vector2(0, 0)));
			return image.ToActionResult();
		}
	}
}
