using Daniel15.Web.Models.Blog;
using Microsoft.AspNetCore.Http.Extensions;

namespace Daniel15.Web.Services.Social
{
	/// <summary>
	/// Support for sharing posts on LinkedIn
	/// </summary>
	public class Linkedin : ISocialShare
	{
		/// <summary>
		/// Base URL for LinkedIn share URLs
		/// </summary>
		private const string SHARE_URL = "http://www.linkedin.com/shareArticle";

		/// <summary>
		/// Gets the internal ID of this social network
		/// </summary>
		public string Id => "linkedin";

		/// <summary>
		/// Gets the friendly name of this social network
		/// </summary>
		public string Name => "LinkedIn";

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
				{"mini", "true"},
				{"url", url},
				{"title", post.Title},
				{"source", "Daniel15"},
			};
		}
		#endregion
	}
}
