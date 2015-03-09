using System;
using System.Collections.Generic;
using Daniel15.Data.Entities.Blog;
using StackExchange.Profiling;

namespace Daniel15.BusinessLayer.Services.Social
{
	/// <summary>
	/// Used to share posts on all available social networks
	/// </summary>
	public class SocialManager : ISocialManager
	{
		private readonly IList<ISocialShare> _socialShares = new List<ISocialShare>();

		/// <summary>
		/// Initializes a new instance of the <see cref="SocialManager" /> class.
		/// </summary>
		public SocialManager(Twitter twitter, Facebook facebook, Linkedin linkedin, Reddit reddit)
		{
			_socialShares.Add(twitter);
			_socialShares.Add(facebook);
			_socialShares.Add(linkedin);
			_socialShares.Add(reddit);
		}

		/// <summary>
		/// Gets the URL to share this post on all available social networks
		/// </summary>
		/// <param name="post">The blog post</param>
		/// <param name="url">Full URL to this post</param>
		/// <param name="shortUrl">Short URL to this post</param>
		/// <returns>Sharing URLs for this post</returns>
		public IEnumerable<KeyValuePair<ISocialNetwork, string>> ShareUrls(PostModel post, string url, string shortUrl)
		{
			foreach (var sharer in _socialShares)
			{
				yield return new KeyValuePair<ISocialNetwork, string>(sharer, sharer.GetShareUrl(post, url, shortUrl));
			}
		}

		/// <summary>
		/// Gets the number of times this URL has been shared on this social network.
		/// </summary>
		/// <param name="post">The blog post</param>
		/// <param name="url">Full URL to this post</param>
		/// <param name="shortUrl">Short URL to this post</param>
		/// <returns>Share count for this post</returns>
		public IDictionary<ISocialNetwork, int> ShareCounts(PostModel post, string url, string shortUrl)
		{
			IDictionary<ISocialNetwork, int> results = new Dictionary<ISocialNetwork, int>(_socialShares.Count);
			var legacyUrl = GetLegacyUrl(post, url);

			foreach (var sharer in _socialShares)
			{
				using (MiniProfiler.Current.Step("Sharing count for " + sharer.Name))
				{
					int count;
					try
					{
						count = sharer.GetShareCount(post, url, shortUrl);
						count += sharer.GetShareCount(post, legacyUrl, string.Empty);
					}
					catch (Exception ex)
					{
						// If an error occured, just set the count to 0 and log it.
						// These aren't overly important - They shouldn't crash the page!
						// TODO: Figure out how to use ELMAH outside of websites
						//Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception("Couldn't get social share count for " + sharer.Name, ex));
						Console.Error.WriteLine("WARNING: Couldn't get social share count for {0}: {1}", sharer.Name, ex);
						count = 0;
					}

					results.Add(sharer, count);
				}
			}

			return results;
		}

		/// <summary>
		/// Gets a legacy URL to the specified blog post (containing /blog/ at the start)
		/// </summary>
		/// <param name="post">Blog post to link to</param>
		/// <param name="currentUrl">Current URL to the blog post</param>
		/// <returns>Legacy URL to this blog post</returns>
		private string GetLegacyUrl(PostModel post, string currentUrl)
		{
			return currentUrl.Replace(post.Date.Year.ToString(), "blog/" + post.Date.Year);
		}
	}
}