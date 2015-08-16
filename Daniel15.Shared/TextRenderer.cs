using System.Drawing;
using Daniel15.Shared.Extensions;

namespace Daniel15.Shared
{
	/// <summary>
	/// Handles rendering text to an image file.
	/// </summary>
	public static class TextRenderer
	{
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
		public static Image RenderText(string text)
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
		public static byte[] RenderTextToBytes(string text)
		{
			return RenderText(text).ToByteArray();
		}
	}
}
