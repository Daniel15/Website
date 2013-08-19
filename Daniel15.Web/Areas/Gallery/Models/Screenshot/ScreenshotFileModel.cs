namespace Daniel15.Web.Areas.Gallery.Models.Screenshot
{
	/// <summary>
	/// Represents a screenshot file or directory.
	/// </summary>
	public class ScreenshotFileModel
	{
		/// <summary>
		/// Relative path of the screenshot
		/// </summary>
		public string RelativePath { get; set; }
		/// <summary>
		/// File name of the screenshot
		/// </summary>
		public string FileName { get; set; }
		/// <summary>
		/// URL to the screenshot
		/// </summary>
		public string Url { get; set; }
		/// <summary>
		/// URL to the screenshot's thumbnail
		/// </summary>
		public string ThumbnailUrl { get; set; }
		/// <summary>
		/// Type of screenshot
		/// </summary>
		public FileType Type { get; set; }

		/// <summary>
		/// Screenshot model types
		/// </summary>
		public enum FileType
		{
			File,
			Directory,
		}
	}
}