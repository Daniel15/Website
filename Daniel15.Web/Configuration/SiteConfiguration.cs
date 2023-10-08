namespace Daniel15.Web.Configuration
{
	/// <summary>
	/// An implementation of <see cref="ISiteConfiguration"/>.
	/// section.
	/// </summary>
	public class SiteConfiguration : ISiteConfiguration
	{
		/// <summary>
		/// Gets the Google Analytics account to use for the site
		/// </summary>
		public string GoogleAnalyticsAccount { get; set; }

		/// <summary>
		/// Gets the name of the blog
		/// </summary>
		public string BlogName { get; set; }

		/// <summary>
		/// Gets the description of the blog
		/// </summary>
		public string BlogDescription { get; set; }

		/// <summary>
		/// Gets the FeedBurner URL for the blog feed
		/// </summary>
		public string FeedBurnerUrl { get; set; }

		/// <summary>
		/// Gets the Disqus shortname used by the blog comments
		/// </summary>
		public string DisqusShortname { get; set; }

		/// <summary>
		/// Gets the Disqus API key (used for syncing comments)
		/// </summary>
		public string DisqusApiKey { get; set; }

		/// <summary>
		/// Gets the Disqus category ID (used for syncing comments)
		/// </summary>
		public int DisqusCategory { get; set; }

		/// <summary>
		/// Gets the URL to the microblog (Tumblr) feed
		/// </summary>
		public string MicroblogFeedUrl { get; set; }

		/// <summary>
		/// Gets information about the Git commit the site is running on
		/// </summary>
		public SiteGitConfiguration Git { get; set; }
	}
}
