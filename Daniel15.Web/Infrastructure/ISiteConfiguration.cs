using System;

namespace Daniel15.Web.Infrastructure
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
		Uri FeedBurnerUrl { get; }

		/// <summary>
		/// Gets the Disqus shortname used by the blog comments
		/// </summary>
		string DisqusShortname { get; }

		/// <summary>
		/// Whether to enable profiling of the website
		/// </summary>
		bool EnableProfiling { get; }
	}
}
