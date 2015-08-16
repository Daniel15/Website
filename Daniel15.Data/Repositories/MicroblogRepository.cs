using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Daniel15.Data.Entities.Blog;
using Daniel15.Shared.Configuration;

namespace Daniel15.Data.Repositories
{
	/// <summary>
	/// Repository for accessing microblog (ie. Tumblr) posts via RSS feed
	/// </summary>
	public class MicroblogRepository : IMicroblogRepository
	{
		private readonly ISiteConfiguration _siteConfiguration;

		/// <summary>
		/// Initializes a new instance of the <see cref="MicroblogRepository" /> class.
		/// </summary>
		/// <param name="siteConfiguration">The site configuration.</param>
		public MicroblogRepository(ISiteConfiguration siteConfiguration)
		{
			_siteConfiguration = siteConfiguration;
		}
		/// <summary>
		/// Gets the latest blog posts
		/// </summary>
		/// <param name="count">Number of posts to return</param>
		/// <param name="offset">Post to start at</param>
		/// <returns>Latest blog posts</returns>
		public IEnumerable<MicroblogPostModel> LatestPosts(int count = 10, int offset = 0)
		{
			XNamespace feedburnerNs = "http://rssnamespace.org/feedburner/ext/1.0";
			var url = _siteConfiguration.MicroblogFeedUrl;
			var xml = XElement.Load(url);

			return xml
				.Element("channel")
				.Elements("item")
				.Skip(offset).Take(count)
				.Select(item => new MicroblogPostModel
				{
					Title = item.Element("title").Value, 
					// Use original link if coming from FeedBurner (so it doesn't go through FeedBurner proxy)
					// Otherwise, use standard RSS link
					Url = (item.Element(feedburnerNs + "origLink") ?? item.Element("link")).Value, 
					Date = DateTime.Parse(item.Element("pubDate").Value)
				});
		}
	}
}
