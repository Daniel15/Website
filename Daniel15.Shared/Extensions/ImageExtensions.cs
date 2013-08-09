using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Daniel15.Shared.Extensions
{
	/// <summary>
	/// Extensions for <see cref="Image"/>s
	/// </summary>
	public static class ImageExtensions
	{
		/// <summary>
		/// Write this image to a byte array
		/// </summary>
		/// <param name="image">Image to write</param>
		/// <param name="format">Format to output</param>
		/// <returns>Image contents as byte array</returns>
		public static byte[] ToByteArray(this Image image, ImageFormat format = null)
		{
			using (var stream = new MemoryStream())
			{
				image.Save(stream, format ?? ImageFormat.Png);
				return stream.ToArray();
			}
		}

		/// <summary>
		/// Generates a thumbnail for the specified image
		/// </summary>
		/// <param name="sourceImg">Image to generate thumbnail for</param>
		/// <param name="maxSize">Maximum size of the thumbnail in either direction</param>
		/// <returns>Thumbnail image</returns>
		public static Bitmap GenerateThumbnail(this Image sourceImg, int maxSize)
		{
			var ratio = (double)sourceImg.Width / (double)sourceImg.Height;

			int width;
			int height;

			// Smaller than the thumbnail size, use the full image
			if (sourceImg.Width < maxSize && sourceImg.Height < maxSize)
			{
				width = sourceImg.Width;
				height = sourceImg.Height;
			}
			else
			{
				// Wider than it is tall
				if (ratio > 1)
				{
					width = maxSize;
					height = (int)(((double)maxSize / sourceImg.Width) * sourceImg.Height);
				}
				// Taller than it is wide
				else
				{
					height = maxSize;
					width = (int)(((double)maxSize / sourceImg.Height) * sourceImg.Width);
				}
			}

			var thumb = new Bitmap(width, height);
			using (var graphics = Graphics.FromImage(thumb))
			{
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.DrawImage(sourceImg, 0, 0, width, height);
			}
			return thumb;
		}
	}
}
