using System.Drawing;
using System.Web.Mvc;
using System.Web.UI;
using Daniel15.Data.Repositories;
using Daniel15.Shared.Extensions;
using System.Linq;

namespace Daniel15.Web.Controllers
{
	/// <summary>
	/// Controller to render various dynamic signature images
	/// </summary>
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
		[OutputCache(Duration = 60 * 5, Location = OutputCacheLocation.Any)]
		public virtual ActionResult LatestBlogPost()
		{
			var latestPost = _blogRepository.LatestPosts(1).First();
			var text = "Latest blog post: " + latestPost.Title;
			return File(RenderTextToBytes(text), "image/png");	
		}

		// TODO: Move this to BusinessLayer
		#region Text rendering
		/// <summary>
		/// Font to use to render signature images
		/// </summary>
		private static readonly Font _font = new Font("Verdana", 9);
		/// <summary>
		/// Background colour of signature images
		/// </summary>
		private static readonly Color _backColour = Color.Transparent;
		/// <summary>
		/// Colour for text in signature images
		/// </summary>
		private static readonly Color _colour = Color.Black;
		/// <summary>
		/// Render the specified string into a PNG image
		/// </summary>
		/// <param name="text">Text to render</param>
		/// <returns>Text in an image</returns>
		private Image RenderText(string text)
		{
			SizeF textSize;

			// First create a dummy bitmap to measure the text
			using (var tempImg = new Bitmap(1, 1))
			using (var drawing = Graphics.FromImage(tempImg))
			{
				textSize = drawing.MeasureString(text, _font);
			}

			var img = new Bitmap((int)textSize.Width, (int)textSize.Height);
			using (var drawing = Graphics.FromImage(img))
			{
				drawing.Clear(_backColour);

				using (var textBrush = new SolidBrush(_colour))
				{
					drawing.DrawString(text, _font, textBrush, 0, 0);
				}

				drawing.Save();
			}
			return img;
		}

		/// <summary>
		/// Render the specified string into a PNG image, and return the image as a byte array
		/// </summary>
		/// <param name="text">Text to render</param>
		/// <returns>Text in an image</returns>
		private byte[] RenderTextToBytes(string text)
		{
			return RenderText(text).ToByteArray();
		}
		#endregion
	}
}
