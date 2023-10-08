namespace Daniel15.Web.Configuration
{
	public class Gallery
	{
		/// <summary>
		/// Gets the name of the image gallery
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets the friendly title of the image gallery
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Gets the URL images in this gallery are directly accessible at
		/// </summary>
		public string ImageUrl { get; set; }

		/// <summary>
		/// Gets the directory images for this gallery are stored in
		/// </summary>
		public string ImageDir { get; set; }
	}
}
