using System.Linq;
using System.Web.Mvc;
using Daniel15.BusinessLayer.Services;
using Daniel15.Data.Entities.Blog;
using Daniel15.Data.Repositories;
using Daniel15.Infrastructure;
using Daniel15.Web.ViewModels.Blog;
using Daniel15.Web.ViewModels.Feed;
using Daniel15.Web.Extensions;

namespace Daniel15.Web.Controllers
{
	/// <summary>
	/// Handles rendering various feeds
	/// </summary>
	public partial class FeedController : Controller
	{
		/// <summary>
		/// Number of blog posts to show in the RSS feed
		/// </summary>
		private const int ITEMS_IN_FEED = 10;

		private readonly IBlogRepository _blogRepository;
		private readonly ISiteConfiguration _siteConfig;
		private readonly IUrlShortener _urlShortener;

		/// <summary>
		/// Initializes a new instance of the <see cref="FeedController" /> class.
		/// </summary>
		/// <param name="blogRepository">The blog repository.</param>
		/// <param name="siteConfig">Site configuration</param>
		/// <param name="urlShortener">URL shortener</param>
		public FeedController(IBlogRepository blogRepository, ISiteConfiguration siteConfig, IUrlShortener urlShortener)
		{
			_blogRepository = blogRepository;
			_siteConfig = siteConfig;
			_urlShortener = urlShortener;
		}

		/// <summary>
		/// Gets a sitemap for the website
		/// </summary>
		/// <returns>Sitemap XML</returns>
		public virtual ActionResult Sitemap()
		{
			Response.ContentType = "text/xml";
			return View(new SitemapViewModel
			{
				Posts = _blogRepository.LatestPostsSummary(10000), // Should be big enough, lols
				Categories = _blogRepository.Categories(),
				Tags = _blogRepository.Tags()
			});
		}

		/// <summary>
		/// RSS feed of all the latest posts
		/// </summary>
		/// <returns>RSS feed</returns>
		public virtual ActionResult BlogLatest()
		{
			if (Request.ShouldRedirectToFeedburner())
				return Redirect(_siteConfig.FeedBurnerUrl.ToString());

			var posts = _blogRepository.LatestPosts(ITEMS_IN_FEED);

			Response.ContentType = "application/rss+xml";
			// Set last-modified date based on the date of the newest post
			Response.Cache.SetLastModified(posts[0].Date);

			return View(new FeedViewModel
			{
				Posts = posts.Select(post => new PostViewModel
				{
					Post = post,
					ShortUrl = ShortUrl(post),
					// TODO: This should be optimised as it will do a SELECT N+1
					// Although FeedBurner will be caching this feed so it's not too significant
					PostCategories = _blogRepository.CategoriesForPost(post)
				}).ToList()
			});
		}

		/// <summary>
		/// Gets the short URL for this blog post
		/// </summary>
		/// <param name="post">Blog post</param>
		/// <returns>The short URL</returns>
		private string ShortUrl(PostModel post)
		{
			return Url.Action(MVC.Blog.ShortUrl(_urlShortener.Shorten(post)), "http");
		}
	}
}
