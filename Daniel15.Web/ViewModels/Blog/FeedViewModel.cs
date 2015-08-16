using System;
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

		/// <summary>
		/// String used as the start of the GUID
		/// </summary>
		public string FeedGuidBase { get; set; }

		/// <summary>
		/// Gets the last modified date/time of the feed. This is the publish date of the latest post,
		/// or <c>DateTime.Now</c> if there are no posts in the feed.
		/// </summary>
		public DateTime LastModified => Posts.Count == 0 ? DateTime.Now : Posts[0].Post.Date;
	}
}
