namespace Daniel15.Infrastructure
{
	/// <summary>
	/// Represents configuration details for the site.
	/// </summary>
	public interface ISiteConfiguration
	{
		/// <summary>
		/// Gets the Google Analytics account to use for the site
		/// </summary>
		string GoogleAnalyticsAccount { get; }

		/// <summary>
		/// Gets the name of the blog
		/// </summary>
		string BlogName { get; }

		/// <summary>
		/// Gets the description of the blog
		/// </summary>
		string BlogDescription { get; }

		/// <summary>
		/// Gets the FeedBurner URL for the blog feed
		/// </summary>
		string FeedBurnerUrl { get; }

		/// <summary>
		/// Gets the Disqus shortname used by the blog comments
		/// </summary>
		string DisqusShortname { get; }

		/// <summary>
		/// Gets the Disqus API key (used for syncing comments)
		/// </summary>
		string DisqusApiKey { get; }

		/// <summary>
		/// Gets the Disqus category ID (used for syncing comments)
		/// </summary>
		int DisqusCategory { get; }

		/// <summary>
		/// Gets the URL to the microblog (Tumblr) feed
		/// </summary>
		string MicroblogFeedUrl { get; }
	}
}
