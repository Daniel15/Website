using System.Drawing;
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
	}
}
