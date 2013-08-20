using System;
using System.Configuration;

namespace Daniel15.Configuration
{
	public interface IGallery
	{
		/// <summary>
		/// Gets the name of the image gallery
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets the friendly title of the image gallery
		/// </summary>
		string Title { get; }

		/// <summary>
		/// Gets the URL images in this gallery are directly accessible at
		/// </summary>
		Uri ImageUrl { get; }

		/// <summary>
		/// Gets the directory images for this gallery are stored in
		/// </summary>
		string ImageDir { get; }
	}
}