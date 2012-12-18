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
	}
}