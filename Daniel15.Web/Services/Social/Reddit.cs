using Daniel15.Web.Models.Blog;
using Microsoft.AspNetCore.Http.Extensions;

namespace Daniel15.Web.Services.Social
{
	/// <summary>
	/// Support for sharing posts on Reddit
	/// </summary>
	public class Reddit : ISocialShare
	{
		/// <summary>
		/// Base URL for Reddit share URLs
		/// </summary>
		private const string SHARE_URL = "http://reddit.com/submit";

		/// <summary>
		/// Gets the internal ID of this social network
		/// </summary>
		public string Id => "reddit";

		/// <summary>
		/// Gets the friendly name of this social network
		/// </summary>
		public string Name => "Reddit";

		#region Implementation of ISocialShare
		/// <summary>
		/// Gets the URL to share this post on this social network.
		/// </summary>
		/// <param name="post">The blog post</param>
		/// <param name="url">Full URL to this post</param>
		/// <param name="shortUrl">Short URL to this post</param>
		/// <returns>Sharing URL for this post</returns>
		public string GetShareUrl(PostModel post, string url, string shortUrl)
		{
			return SHARE_URL + new QueryBuilder
			{
				{"url", url},
				{"title", post.Title}
			};
		}
		#endregion
	}
}
