using Daniel15.Web.Models.Blog;

namespace Daniel15.Web.Services.Social
{
	/// <summary>
	/// Represents a social network that can be used by users to share posts
	/// </summary>
	public interface ISocialShare : ISocialNetwork
	{
		/// <summary>
		/// Gets the URL to share this post on this social network.
		/// </summary>
		/// <param name="post">The blog post</param>
		/// <param name="url">Full URL to this post</param>
		/// <param name="shortUrl">Short URL to this post</param>
		/// <returns>Sharing URL for this post</returns>
		string GetShareUrl(PostModel post, string url, string shortUrl);

		/// <summary>
		/// Gets the number of times this URL has been shared on this social network.
		/// </summary>
		/// <param name="post">The blog post</param>
		/// <param name="url">Full URL to this post</param>
		/// <param name="shortUrl">Short URL to this post</param>
		/// <returns>Share count for this post</returns>
		Task<int> GetShareCountAsync(PostModel post, string url, string shortUrl);
	}
}
