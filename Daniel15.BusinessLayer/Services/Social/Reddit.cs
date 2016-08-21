using System;
using System.Net;
using Daniel15.Data.Entities.Blog;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json.Linq;

namespace Daniel15.BusinessLayer.Services.Social
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
		/// URL to retrieve sharing count
		/// </summary>
		private const string API_URL = "http://www.reddit.com/api/info.json";

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

		/// <summary>
		/// Gets the number of times this URL has been shared on this social network.
		/// </summary>
		/// <param name="post">The blog post</param>
		/// <param name="url">Full URL to this post</param>
		/// <param name="shortUrl">Short URL to this post</param>
		/// <returns>Share count for this post</returns>
		public int GetShareCount(PostModel post, string url, string shortUrl)
		{
			var total = 0;
			var apiUrl = API_URL + new QueryBuilder
			{
				{"url", url}
			};

			using (var client = new WebClient())
			{
				var rawData = client.DownloadString(apiUrl);
				dynamic data = JObject.Parse(rawData);
				if (data == null || data.data == null || data.data.children == null)
					return 0;

				// Need to add up the points in every submission of this URL
				foreach (var child in data.data.children)
				{
					total += Convert.ToInt32(child.data.score);
				}

				return total;
			}
		}
		#endregion
	}
}
