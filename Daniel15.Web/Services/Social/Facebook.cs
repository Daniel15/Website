using Daniel15.Web.Models.Blog;
using Microsoft.AspNetCore.Http.Extensions;

namespace Daniel15.Web.Services.Social
{
	/// <summary>
	/// Support for sharing posts on Facebook
	/// </summary>
	public class Facebook : ISocialShare
	{
		/// <summary>
		/// Base URL for Facebook share URLs
		/// </summary>
		private const string SHARE_URL = "https://www.facebook.com/sharer.php";

		/// <summary>
		/// Gets the internal ID of this social network
		/// </summary>
		public string Id => "facebook";

		/// <summary>
		/// Gets the friendly name of this social network
		/// </summary>
		public string Name => "Facebook";

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
				{"u", url},
				{"t", post.Title},
			};
		}
		#endregion
	}
}
