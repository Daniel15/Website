using System;
using System.Net.Http;
using System.Threading.Tasks;
using Daniel15.Web.Models.Blog;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json.Linq;

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
		/// API URL to get sharing counts
		/// </summary>
		private const string API_COUNT_URL = "http://www.linkedin.com/countserv/count/share";

		private readonly HttpClient _client;

		/// <summary>
		/// Gets the internal ID of this social network
		/// </summary>
		public string Id => "linkedin";

		/// <summary>
		/// Gets the friendly name of this social network
		/// </summary>
		public string Name => "LinkedIn";

		public Linkedin(HttpClient client)
		{
			_client = client;
		}

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

		/// <summary>
		/// Gets the number of times this URL has been shared on this social network.
		/// </summary>
		/// <param name="post">The blog post</param>
		/// <param name="url">Full URL to this post</param>
		/// <param name="shortUrl">Short URL to this post</param>
		/// <returns>Share count for this post</returns>
		public async Task<int> GetShareCountAsync(PostModel post, string url, string shortUrl)
		{
			var apiUrl = API_COUNT_URL + new QueryBuilder
			{
				{"url", url}
			};

			var responseText = await _client.GetStringAsync(apiUrl);
			// Ugly hack to get JSON data from the JavaScript method call
			// This API call is usually used client-side via JSONP...
			responseText = responseText.Replace("IN.Tags.Share.handleCount(", "").Replace(");", "");
			dynamic response = JObject.Parse(responseText);

			if (response == null || response.count == null)
				return 0;

			return Convert.ToInt32(response.count);
		}
		#endregion
	}
}
