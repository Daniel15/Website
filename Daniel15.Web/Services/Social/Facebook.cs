using System;
using System.Threading.Tasks;
using System.Xml.Linq;
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
		/// API URL for FQL queries
		/// </summary>
		private const string QUERY_URL = "http://api.facebook.com/method/fql.query";
		/// <summary>
		/// Query to get sharing counts
		/// </summary>
		private const string LINK_QUERY = "SELECT share_count, like_count, comment_count, total_count FROM link_stat WHERE url=\"{0}\"";

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

		/// <summary>
		/// Gets the number of times this URL has been shared on this social network.
		/// </summary>
		/// <param name="post">The blog post</param>
		/// <param name="url">Full URL to this post</param>
		/// <param name="shortUrl">Short URL to this post</param>
		/// <returns>Share count for this post</returns>
		public Task<int> GetShareCountAsync(PostModel post, string url, string shortUrl)
		{
			// Get the count using FQL. Returns *both* like count and share count.
			var query = string.Format(LINK_QUERY, url);
			var queryUrl = QUERY_URL + new QueryBuilder
			{
				{"query", query}
			};

			var xml = XDocument.Load(queryUrl);
			XNamespace ns = "http://api.facebook.com/1.0/";

			var linkStat = xml.Root?.Element(ns + "link_stat");
			if (linkStat == null)
				return Task.FromResult(0);

			var totalCount = linkStat.Element(ns + "total_count");
			return Task.FromResult(totalCount == null ? 0 : Convert.ToInt32(totalCount.Value));
		}
		#endregion
	}
}
