using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Daniel15.Web.Models.Blog;
using Daniel15.Web.Repositories;
using Daniel15.Web.Services;
using Daniel15.Web.Services.Social;
using Daniel15.Web.ViewModels.Blog;
using Daniel15.Web.Extensions;
using System.Linq;
using StackExchange.Profiling;

namespace Daniel15.Web.Controllers
{
	/// <summary>
	/// Controller for the main blog pages
	/// </summary>
	public partial class BlogController : Controller
	{
		/// <summary>
		/// Number of blog posts to show on each page
		/// </summary>
		private const int ITEMS_PER_PAGE = 10;
		/// <summary>
		/// Number of blog posts to show in the RSS feed
		/// </summary>
		private const int ITEMS_IN_FEED = 10;

		private readonly IBlogRepository _blogRepository;
		private readonly IUrlShortener _urlShortener;
		private readonly ISocialManager _socialManager;
		private readonly MiniProfiler _profiler;

		/// <summary>
		/// Initializes a new instance of the <see cref="BlogController" /> class.
		/// </summary>
		/// <param name="blogRepository">The blog post repository.</param>
		/// <param name="urlShortener">The URL shortener</param>
		/// <param name="socialManager">The social network manager used to get sharing URLs</param>
		public BlogController(IBlogRepository blogRepository, IUrlShortener urlShortener, ISocialManager socialManager)
		{
			_blogRepository = blogRepository;
			_urlShortener = urlShortener;
			_socialManager = socialManager;
			_profiler = MiniProfiler.Current;
		}

		/// <summary>
		/// Returns a listing of blog posts
		/// </summary>
		/// <param name="posts">Posts to be displayed</param>
		/// <param name="count">Number of posts being displayed</param>
		/// <param name="page">Page number of the current page</param>
		/// <param name="viewName">Name of the view to render</param>
		/// <param name="viewModel">View model to pass to the view</param>
		/// <returns>Post listing</returns>
		private ActionResult Listing(IEnumerable<PostModel> posts, int count, int page, string viewName = null, ListingViewModel viewModel = null)
		{
			if (viewName == null)
				viewName = Views.Index;

			if (viewModel == null)
				viewModel = new ListingViewModel();

			var pages = (int)Math.Ceiling((double)count / ITEMS_PER_PAGE);

			using (_profiler.Step("Building post ViewModels"))
			{
				viewModel.Posts = posts.Select(post => new PostViewModel
				{
					Post = post, 
					ShortUrl = ShortUrl(post),
					SocialNetworks = _socialManager.ShareUrls(post, Url.BlogAbsolute(post), ShortUrl(post))
				});
			}
			viewModel.TotalCount = count;
			viewModel.Page = page;
			viewModel.TotalPages = pages;
			return View(viewName, viewModel);
		}

		/// <summary>
		/// Index page of the blog
		/// </summary>
		public virtual ActionResult Index(int page = 1)
		{
			var count = _blogRepository.PublishedCount();
			var posts = _blogRepository.LatestPosts(ITEMS_PER_PAGE, (page - 1) * ITEMS_PER_PAGE);
			return Listing(posts, count, page, Views.Index);
		}

		/// <summary>
		/// Viewing a category listing
		/// </summary>
		/// <param name="slug">Category slug</param>
		/// <param name="page">Page number to view</param>
		/// <returns>Posts in this category</returns>
		public virtual ActionResult Category(string slug, int page = 1)
		{
			CategoryModel category;
			try
			{
				category = _blogRepository.GetCategory(slug);
			}
			catch (ItemNotFoundException)
			{
				// Throw a 404 if the category doesn't exist
				return HttpNotFound(string.Format("Category '{0}' not found.", slug));
			}

			var count = _blogRepository.PublishedCount(category);
			var posts = _blogRepository.LatestPosts(category, ITEMS_PER_PAGE, (page - 1) * ITEMS_PER_PAGE);
			return Listing(posts, count, page, Views.Category, new CategoryListingViewModel { Category = category });
		}

		/// <summary>
		/// Viewing a listing of all posts tagged by a particular tag
		/// </summary>
		/// <param name="slug">Tag slug</param>
		/// <param name="page">Page number to view</param>
		/// <returns>Posts tagged with this tag</returns>
		public virtual ActionResult Tag(string slug, int page = 1)
		{
			// TODO
			throw new NotImplementedException();
		}

		/// <summary>
		/// Viewing the blog archive (articles posted in the specified year and month)
		/// </summary>
		/// <param name="year">Year to get posts for</param>
		/// <param name="month">Month to get posts for</param>
		/// <param name="page">Page number to view</param>
		/// <returns>Posts from this month</returns>
		public virtual ActionResult Archive(int year, int month, int page = 1)
		{
			var count = _blogRepository.PublishedCountForMonth(year, month);
			var posts = _blogRepository.LatestPostsForMonth(year, month, ITEMS_PER_PAGE, (page - 1) * ITEMS_PER_PAGE);
			return Listing(posts, count, page, Views.Index);
		}

		/// <summary>
		/// Viewing a blog post
		/// </summary>
		/// <param name="month">The month of the post</param>
		/// <param name="year">The year of the post</param>
		/// <param name="slug">The slug.</param>
		/// <returns>Blog post page</returns>
		public virtual ActionResult View(int month, int year, string slug)
		{
			PostModel post;
			try
			{
				post = _blogRepository.GetBySlug(slug);
			}
			catch (ItemNotFoundException)
			{
				// Throw a 404 if the post doesn't exist
				return HttpNotFound(string.Format("Blog post '{0}' not found.", slug));
			}

			// Check the URL was actually correct (year and month), redirect if not.
			if (year != post.Date.Year || month != post.Date.Month)
			{
				return RedirectPermanent(Url.Blog(post));
			}

			// Set last-modified date based on the date of the post
			Response.Cache.SetLastModified(post.Date);

			return View(new PostViewModel
			{
				Post = post,
				PostCategories = _blogRepository.CategoriesForPost(post),
				PostTags = _blogRepository.TagsForPost(post),
				ShortUrl = Url.Action(MVC.Blog.ShortUrl(_urlShortener.Shorten(post)), "http"),
				SocialNetworks = _socialManager.ShareUrls(post, Url.BlogAbsolute(post), ShortUrl(post))
			});
		}

		/// <summary>
		/// Short URL redirect - Looks up a short URL and redirects to the post
		/// </summary>
		/// <param name="alias">URL alias</param>
		/// <returns>Redirect to correct post</returns>
		public virtual ActionResult ShortUrl(string alias)
		{
			var id = _urlShortener.Extend(alias);
			PostModel post;
			try
			{
				// TODO: Add a GetSummary method and use that instead
				post = _blogRepository.Get(id);
			}
			catch (Exception)
			{
				return HttpNotFound(string.Format("Blog post {0} for short URL '{1}' not found.", id, alias));
			}

			return RedirectPermanent(Url.Blog(post));
		}

		/// <summary>
		/// RSS feed of all the latest posts
		/// </summary>
		/// <returns>RSS feed</returns>
		public virtual ActionResult Feed()
		{
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
