using System.Collections.Generic;
using Daniel15.Data.Entities.Blog;

namespace Daniel15.Web.Services.Social
{
	/// <summary>
	/// Used to share posts on all available social networks
	/// </summary>
	public interface ISocialManager
	{
		/// <summary>
		/// Gets the URL to share this post on all available social networks
		/// </summary>
		/// <param name="post">The blog post</param>
		/// <param name="url">Full URL to this post</param>
		/// <param name="shortUrl">Short URL to this post</param>
		/// <returns>Sharing URLs for this post</returns>
		IEnumerable<KeyValuePair<ISocialNetwork, string>> ShareUrls(PostSummaryModel post, string url, string shortUrl);

		/// <summary>
		/// Gets the number of times this URL has been shared on this social network.
		/// </summary>
		/// <param name="post">The blog post</param>
		/// <param name="url">Full URL to this post</param>
		/// <param name="shortUrl">Short URL to this post</param>
		/// <returns>Share count for this post</returns>
		IDictionary<ISocialNetwork, int> ShareCounts(PostSummaryModel post, string url, string shortUrl);
	}
}
