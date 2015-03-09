using System;
using System.Collections.Generic;
using System.Web.Helpers;
using Daniel15.Data.Entities.Blog;
using Daniel15.Shared.Extensions;
using ServiceStack.Text;

namespace Daniel15.BusinessLayer.Services.Social
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

		/// <summary>
		/// Gets the internal ID of this social network
		/// </summary>
		public string Id { get { return "linkedin"; } }

		/// <summary>
		/// Gets the friendly name of this social network
		/// </summary>
		public string Name { get { return "LinkedIn"; } }

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
			return SHARE_URL + "?" + new Dictionary<string, object>
			{
				{"mini", "true"},
				{"url", url},
				{"title", post.Title},
				{"source", "Daniel15"},
			}.ToQueryString();
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
			var apiUrl = API_COUNT_URL + "?" + new Dictionary<string, object>
			{
				{"url", url}
			}.ToQueryString();

			var responseText = apiUrl.GetJsonFromUrl();
			// Ugly hack to get JSON data from the JavaScript method call
			// This API call is usually used client-side via JSONP...
			responseText = responseText.Replace("IN.Tags.Share.handleCount(", "").Replace(");", "");
			var response = Json.Decode(responseText);

			if (response == null || response.count == null)
				return 0;

			return Convert.ToInt32(response.count);
		}
		#endregion
	}
}