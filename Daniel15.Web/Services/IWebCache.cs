using Daniel15.Web.Models.Blog;

namespace Daniel15.Web.Services
{
	/// <summary>
	/// Manages the front-end cache for the site
	/// </summary>
	public interface IWebCache
	{
		/// <summary>
		/// Clear all relevant caches for this blog post
		/// </summary>
		/// <param name="post">The blog post</param>
		void ClearCache(PostSummaryModel post);
	}
}
