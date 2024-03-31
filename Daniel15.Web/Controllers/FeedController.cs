using Daniel15.Web.Services;
using Daniel15.Web.Models.Blog;
using Daniel15.Web.Repositories;
using Daniel15.Web.Configuration;
using Daniel15.Web.Exceptions;
using Daniel15.Web.Extensions;
using Daniel15.Web.ViewModels.Blog;
using Daniel15.Web.ViewModels.Feed;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

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
		private readonly IProjectRepository _projectRepository;
		private readonly ISiteConfiguration _siteConfig;
		private readonly IUrlShortener _urlShortener;

		/// <summary>
		/// Initializes a new instance of the <see cref="FeedController" /> class.
		/// </summary>
		/// <param name="blogRepository">The blog repository.</param>
		/// <param name="projectRepository">Project repository</param>
		/// <param name="siteConfig">Site configuration</param>
		/// <param name="urlShortener">URL shortener</param>
		public FeedController(IBlogRepository blogRepository, IProjectRepository projectRepository, ISiteConfiguration siteConfig, IUrlShortener urlShortener)
		{
			_blogRepository = blogRepository;
			_projectRepository = projectRepository;
			_siteConfig = siteConfig;
			_urlShortener = urlShortener;
		}

		/// <summary>
		/// Gets a sitemap for the website
		/// </summary>
		/// <returns>Sitemap XML</returns>
		[Route("sitemap.xml")]
		[OutputCache]
		public virtual ActionResult Sitemap()
		{
			var view = View(new SitemapViewModel
			{
				Posts = _blogRepository.LatestPosts(10000), // Should be big enough, lols
				Categories = _blogRepository.Categories(),
				Tags = _blogRepository.Tags(),
				Projects = _projectRepository.All()
			});
			view.ContentType = "text/xml";
			return view;
		}

		/// <summary>
		/// RSS feed of all the latest posts
		/// </summary>
		/// <returns>RSS feed</returns>
		[Route("blog/feed")]
		[OutputCache]
		public virtual ActionResult BlogLatest()
		{
			var posts = _blogRepository.LatestPosts(ITEMS_IN_FEED);
			return RenderFeed(posts, new FeedViewModel
			{
				FeedGuidBase = "Latest",
				Title = _siteConfig.BlogName,
				Description = _siteConfig.BlogDescription,
				FeedUrl = Url.Absolute(Url.Action("BlogLatest")),
				SiteUrl = Url.Action("Index", "Blog", null, Request.Scheme),
			});
		}

		/// <summary>
		/// RSS feed for a specific category
		/// </summary>
		/// <param name="slug">Category slug</param>
		/// <param name="parentSlug">Slug of the category's parent</param>
		/// <returns>RSS feed</returns>
		[Route("category/{parentSlug}/{slug}.rss", Order = 1, Name = "BlogSubCategoryFeed")]
		[Route("category/{slug}.rss", Order = 2, Name = "BlogCategoryFeed")]
		[OutputCache]
		public virtual ActionResult BlogCategory(string slug, string parentSlug = null)
		{
			CategoryModel category;
			try
			{
				category = _blogRepository.GetCategory(slug);
			}
			catch (EntityNotFoundException)
			{
				// Throw a 404 if the category doesn't exist
				return NotFound();
			}

			// If the category has a parent category, ensure it's in the URL
			if (category.Parent != null && string.IsNullOrEmpty(parentSlug))
			{
				return RedirectToActionPermanent("BlogCategory", "Feed", new { slug, parentSlug = category.Parent.Slug });
			}

			var posts = _blogRepository.LatestPosts(category, ITEMS_IN_FEED);
			return RenderFeed(posts, new FeedViewModel
			{
				FeedGuidBase = "Category-" + category.Slug + "-",
				Title = category.Title + " - " + _siteConfig.BlogName,
				Description = category.Title + " posts to " + _siteConfig.BlogName,
				FeedUrl = Url.Absolute(Url.BlogCategoryFeed(category)),
				SiteUrl = Url.Absolute(Url.BlogCategory(category))
			});
		}

		/// <summary>
		/// Renders the specified list of posts to an RSS feed
		/// </summary>
		/// <param name="posts">Posts to render</param>
		/// <param name="model">View model to render with</param>
		/// <returns>RSS feed</returns>
		private ActionResult RenderFeed(IList<PostModel> posts, FeedViewModel model)
		{
			// Set last-modified date based on the date of the newest post
			if (posts.Count > 0)
			{
				Response.Headers["Last-Modified"] = posts[0].Date.ToString("R");
			}

			var categories = _blogRepository.CategoriesForPosts(posts);
			model.Posts = posts.Select(post => new PostViewModel
			{
				Post = post,
				ShortUrl = ShortUrl(post),
				PostCategories = categories[post]
			}).ToList();

			return View("Blog", model);
		}

		/// <summary>
		/// Gets the short URL for this blog post
		/// </summary>
		/// <param name="post">Blog post</param>
		/// <returns>The short URL</returns>
		private string ShortUrl(PostModel post)
		{
			return Url.Action("Blog", "ShortUrl", new { alias = _urlShortener.Shorten(post) }, Request.Scheme);
		}
	}
}
