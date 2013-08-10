using System;
using System.Diagnostics;
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

			return sourceImg.GenerateMagickThumbnail(width, height);
		}

		/// <summary>
		/// Generates a thumbnail for the specified image, using GDI+
		/// </summary>
		/// <param name="sourceImg">Image to generate thumbnail for</param>
		/// <param name="width">Width of the thumbnail</param>
		/// <param name="height">Height of the thumbnail</param>
		/// <returns>Thumbnail image</returns>
		public static Bitmap GenerateGdiPlusThumbnail(this Image sourceImg, int width, int height)
		{
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

		/// <summary>
		/// Generates a thumbnail for the specified image, using ImageMagick or GraphicsMagick
		/// </summary>
		/// <param name="sourceImg">Image to generate thumbnail for</param>
		/// <param name="width">Width of the thumbnail</param>
		/// <param name="height">Height of the thumbnail</param>
		/// <returns>Thumbnail image</returns>
		public static Bitmap GenerateMagickThumbnail(this Image sourceImg, int width, int height)
		{
			// Create new GraphicsMagick process for thumbnail generation
			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "gm",
					Arguments = string.Format("convert - -filter sinc -size {0}x{1} -resize {0}x{1} -", width, height),
					RedirectStandardInput = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true,
				}
			};

			process.Start();

			// Write source image to input stream of GraphicsMagick
			sourceImg.Save(process.StandardInput.BaseStream, ImageFormat.Png);
			process.StandardInput.Flush();
			process.StandardInput.Close();

			try
			{
				var thumb = new Bitmap(process.StandardOutput.BaseStream);
				return thumb;
			}
			catch (Exception ex)
			{
				var errors = process.StandardError.ReadToEnd();
				throw new Exception(string.Format("Error invoking GraphicsMagick: {0}\nOriginal exception: {1}", errors, ex));
			}
			finally
			{
				process.Dispose();
			}
		}
	}
}
