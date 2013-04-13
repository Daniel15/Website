using System.Collections.Generic;

namespace Daniel15.Web.ViewModels.Blog
{
	/// <summary>
	/// Data required to render the blog RSS feed.
	/// </summary>
	public class FeedViewModel
	{
		/// <summary>
		/// Posts to display on the current blog page
		/// </summary>
		public IList<PostViewModel> Posts { get; set; }

		/// <summary>
		/// Title of the RSS feed
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Description of the RSS feed
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// URL to the RSS feed
		/// </summary>
		public string FeedUrl { get; set; }

		/// <summary>
		/// URL to the site
		/// </summary>
		public string SiteUrl { get; set; }
	}
}