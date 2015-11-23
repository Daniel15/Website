using System;
using System.Net;
using Daniel15.Data.Entities.Blog;
using Microsoft.AspNet.Http.Extensions;
using Newtonsoft.Json.Linq;

namespace Daniel15.BusinessLayer.Services.Social
{
	/// <summary>
	/// Support for sharing posts on Twitter
	/// </summary>
	public class Twitter : ISocialShare
	{
		/// <summary>
		/// Base URL for Twitter share URLs
		/// </summary>
		private const string SHARE_URL = "https://twitter.com/intent/tweet";

		/// <summary>
		/// Gets the internal ID of this social network
		/// </summary>
		public string Id => "twitter";

		/// <summary>
		/// Gets the friendly name of this social network
		/// </summary>
		public string Name => "Twitter";

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
				{"text", post.Title},
				{"original_referer", url},
				{"url", shortUrl},
				{"via", "Daniel15"},
				{"related", "Daniel15"}
			};
		}

		/// <summary>
		/// Gets the number of times this URL has been shared on this social network.
		/// </summary>
		/// <param name="post">The blog post</param>
		/// <param name="url">Full URL to this post</param>
		/// <param name="shortUrl">Short URL to this post</param>
		/// <returns>Share count for this post</returns>
		public int GetShareCount(PostModel post, string url, string shortUrl)
		{
			// No longer supported :(
			// https://blog.twitter.com/2015/hard-decisions-for-a-sustainable-platform
			return 0;
		}
		#endregion
	}
}
