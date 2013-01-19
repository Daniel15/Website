using System;
using System.Configuration;

namespace Daniel15.Web.Infrastructure
{
	/// <summary>
	/// An implementation of <see cref="ISiteConfiguration"/> using a Web.config configuration 
	/// section.
	/// </summary>
	public class SiteConfiguration : ConfigurationSection, ISiteConfiguration
	{
		/// <summary>
		/// Gets the Google Analytics account to use for the site
		/// </summary>
		[ConfigurationProperty("googleAnalyticsAccount", IsRequired = false)]
		public string GoogleAnalyticsAccount
		{
			get { return (string)this["googleAnalyticsAccount"]; }
			set { this["googleAnalyticsAccount"] = value; }
		}

		/// <summary>
		/// Gets the name of the blog
		/// </summary>
		[ConfigurationProperty("blogName", IsRequired = true)]
		public string BlogName
		{
			get { return (string)this["blogName"]; }
			set { this["blogName"] = value; }
		}

		/// <summary>
		/// Gets the description of the blog
		/// </summary>
		[ConfigurationProperty("blogDescription", IsRequired = true)]
		public string BlogDescription
		{
			get { return (string)this["blogDescription"]; }
			set { this["blogDescription"] = value; }
		}

		/// <summary>
		/// Gets the FeedBurner URL for the blog feed
		/// </summary>
		[ConfigurationProperty("feedBurnerUrl", IsRequired = true)]
		public Uri FeedBurnerUrl
		{
			get { return (Uri)this["feedBurnerUrl"]; }
			set { this["feedBurnerUrl"] = value; }
		}

		/// <summary>
		/// Gets the Disqus shortname used by the blog comments
		/// </summary>
		[ConfigurationProperty("disqusShortname", IsRequired = true)]
		public string DisqusShortname
		{
			get { return (string)this["disqusShortname"]; }
			set { this["disqusShortname"] = value; }
		}

		/// <summary>
		/// Gets the URL to the microblog (Tumblr) feed
		/// </summary>
		[ConfigurationProperty("microblogFeedUrl", IsRequired = false)]
		public Uri MicroblogFeedUrl
		{
			get { return (Uri)this["microblogFeedUrl"]; }
			set { this["microblogFeedUrl"] = value; }
		}
	}
}