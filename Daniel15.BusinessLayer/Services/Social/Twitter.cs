using System;
using System.Collections.Generic;
using System.Web.Helpers;
using Daniel15.Data.Entities.Blog;
using Daniel15.Shared.Extensions;
using ServiceStack.Text;

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
		/// URL to retrieve sharing count
		/// </summary>
		private const string COUNT_URL = "http://urls.api.twitter.com/1/urls/count.json";

		/// <summary>
		/// Gets the internal ID of this social network
		/// </summary>
		public string Id { get { return "twitter"; } }

		/// <summary>
		/// Gets the friendly name of this social network
		/// </summary>
		public string Name { get { return "Twitter"; } }

		#region Implementation of ISocialShare
		/// <summary>
		/// Gets the URL to share this post on this social network.
		/// </summary>
		/// <param name="post">The blog post</param>
		/// <param name="url">Full URL to this post</param>
		/// <param name="shortUrl">Short URL to this post</param>
		/// <returns>Sharing URL for this post</returns>
		public string GetShareUrl(PostSummaryModel post, string url, string shortUrl)
		{
			return SHARE_URL + "?" + new Dictionary<string, object>
			{
				{"text", post.Title},
				{"original_referer", url},
				{"url", shortUrl},
				{"via", "Daniel15"},
				{"related", "Daniel15"}
			}.ToQueryString();
		}

		/// <summary>
		/// Gets the number of times this URL has been shared on this social network.
		/// </summary>
		/// <param name="post">The blog post</param>
		/// <param name="url">Full URL to this post</param>
		/// <param name="shortUrl">Short URL to this post</param>
		/// <returns>Share count for this post</returns>
		public int GetShareCount(PostSummaryModel post, string url, string shortUrl)
		{
			var countUrl = COUNT_URL + "?" + new Dictionary<string, object>
			{
				{"url", url},
			}.ToQueryString();

			var response = Json.Decode(countUrl.GetJsonFromUrl());
			if (response == null || response.count == null)
				return 0;

			return Convert.ToInt32(response.count);
		}
		#endregion
	}
}