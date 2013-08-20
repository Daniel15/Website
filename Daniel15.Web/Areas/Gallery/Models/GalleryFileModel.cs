namespace Daniel15.Web.Areas.Gallery.Models
{
	/// <summary>
	/// Represents a gallery file or directory.
	/// </summary>
	public class GalleryFileModel
	{
		/// <summary>
		/// Relative path of the item
		/// </summary>
		public string RelativePath { get; set; }
		/// <summary>
		/// File name of the item
		/// </summary>
		public string FileName { get; set; }
		/// <summary>
		/// URL to the item
		/// </summary>
		public string Url { get; set; }
		/// <summary>
		/// URL to the item's thumbnail
		/// </summary>
		public string ThumbnailUrl { get; set; }
		/// <summary>
		/// Type of item
		/// </summary>
		public FileType Type { get; set; }

		/// <summary>
		/// Gallery file model types
		/// </summary>
		public enum FileType
		{
			File,
			Directory,
		}
	}
}