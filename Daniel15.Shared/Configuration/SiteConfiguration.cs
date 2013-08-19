using System;
using System.ComponentModel;
using System.Configuration;

namespace Daniel15.Infrastructure
{
	/// <summary>
	/// An implementation of <see cref="ISiteConfiguration"/> using a Web.config configuration 
	/// section.
	/// </summary>
	public class SiteConfiguration : ConfigurationSection, ISiteConfiguration
	{
		public ApiKeysConfiguration ApiKeys { get; set; }

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
		[ConfigurationProperty("blogName", IsRequired = false)]
		public string BlogName
		{
			get { return (string)this["blogName"]; }
			set { this["blogName"] = value; }
		}

		/// <summary>
		/// Gets the description of the blog
		/// </summary>
		[ConfigurationProperty("blogDescription", IsRequired = false)]
		public string BlogDescription
		{
			get { return (string)this["blogDescription"]; }
			set { this["blogDescription"] = value; }
		}

		/// <summary>
		/// Gets the FeedBurner URL for the blog feed
		/// </summary>
		[ConfigurationProperty("feedBurnerUrl", IsRequired = false)]
		public Uri FeedBurnerUrl
		{
			get { return (Uri)this["feedBurnerUrl"]; }
			set { this["feedBurnerUrl"] = value; }
		}

		/// <summary>
		/// Gets the Disqus shortname used by the blog comments
		/// </summary>
		[ConfigurationProperty("disqusShortname", IsRequired = false)]
		public string DisqusShortname
		{
			get { return (string)this["disqusShortname"]; }
			set { this["disqusShortname"] = value; }
		}

		/// <summary>
		/// Gets the Disqus API key (used for syncing comments)
		/// </summary>
		public string DisqusApiKey
		{
			get { return ApiKeys.DisqusApiKey; }
		}

		/// <summary>
		/// Gets the Disqus category ID (used for syncing comments)
		/// </summary>
		[ConfigurationProperty("disqusCategory", IsRequired = false)]
		public int DisqusCategory
		{
			get { return (int)this["disqusCategory"]; }
			set { this["disqusCategory"] = value; }
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

		/// <summary>
		/// Gets the type of web cache being used for the site
		/// </summary>
		[ConfigurationProperty("webCacheType", IsRequired = false)]
		[TypeConverter(typeof(TypeNameConverter))]
		public Type WebCacheType
		{
			get { return (Type)this["webCacheType"]; }
			set { this["webCacheType"] = value; }
		}
	}

	/// <summary>
	/// API key configuration section. Separated so that the API keys may be in a separate XML file
	/// outside of source control.
	/// </summary>
	public class ApiKeysConfiguration : ConfigurationSection
	{
		/// <summary>
		/// Gets the Disqus API key (used for syncing comments)
		/// </summary>
		[ConfigurationProperty("disqusApiKey", IsRequired = false)]
		public string DisqusApiKey
		{
			get { return (string)this["disqusApiKey"]; }
			set { this["disqusApiKey"] = value; }
		}
	}
}