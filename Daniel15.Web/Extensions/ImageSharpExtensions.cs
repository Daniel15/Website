using System.IO;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Daniel15.Web.Extensions
{
	/// <summary>
	/// Extension methods for ImageSharp
	/// </summary>
	public static class ImageSharpExtensions
	{
		/// <summary>
		/// Wraps the specified image in an MVC ActionResult
		/// </summary>
		/// <param name="image">Image to return</param>
		/// <returns>ActionResult containing this image</returns>
		public static ActionResult ToActionResult(this Image image)
		{
			// Must NOT dispose this stream - The MVC framework disposes it for us.
			var stream = new MemoryStream();
			image.SaveAsPng(stream);
			stream.Seek(0, SeekOrigin.Begin);
			return new FileStreamResult(stream, "image/png");
		}
    }
}
