using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using Daniel15.BusinessLayer.Services;
using Daniel15.BusinessLayer.Services.Social;
using Daniel15.Data;
using Daniel15.Data.Entities.Blog;
using Daniel15.Data.Repositories;
using Daniel15.Infrastructure;
using Daniel15.Web.Models.Blog;
using Daniel15.Web.ViewModels.Blog;
using Daniel15.Web.Extensions;
using System.Linq;
using ServiceStack;
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
		/// One hour in seconds.
		/// </summary>
		private const int ONE_HOUR = 3600;

		private readonly IBlogRepository _blogRepository;
		private readonly IDisqusCommentRepository _commentRepository;
		private readonly IUrlShortener _urlShortener;
		private readonly ISocialManager _socialManager;
		private readonly MiniProfiler _profiler;

		/// <summary>
		/// Initializes a new instance of the <see cref="BlogController" /> class.
		/// </summary>
		/// <param name="blogRepository">The blog post repository.</param>
		/// <param name="commentRepository">The Disqus comment repository</param>
		/// <param name="urlShortener">The URL shortener</param>
		/// <param name="socialManager">The social network manager used to get sharing URLs</param>
		/// <param name="siteConfig">Site configuration</param>
		public BlogController(IBlogRepository blogRepository, IDisqusCommentRepository commentRepository, IUrlShortener urlShortener, ISocialManager socialManager)
		{
			_blogRepository = blogRepository;
			_commentRepository = commentRepository;
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

			if (page > pages)
				return HttpNotFound(string.Format("Requested page number ({0}) is greater than page count ({1})", page, count));

			using (_profiler.Step("Building post ViewModels"))
			{
				viewModel.Posts = posts.Select(post => new PostViewModel
				{
					Post = post, 
					ShortUrl = ShortUrl(post),
					SocialNetworks = GetSocialNetworks(post)
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
		[OutputCache(Location = OutputCacheLocation.Downstream, Duration = ONE_HOUR, VaryByParam = "page")]
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
		public virtual ActionResult Category(string slug, int page = 1, string parentSlug = null)
		{
			CategoryModel category;
			try
			{
				category = _blogRepository.GetCategory(slug);
			}
			catch (EntityNotFoundException)
			{
				// Throw a 404 if the category doesn't exist
				return HttpNotFound(string.Format("Category '{0}' not found.", slug));
			}

			// If the category has a parent category, ensure it's in the URL
			if (!string.IsNullOrEmpty(category.ParentSlug) && string.IsNullOrEmpty(parentSlug))
			{
				return RedirectPermanent(Url.BlogCategory(category, page));
			}

			var count = _blogRepository.PublishedCount(category);
			var posts = _blogRepository.LatestPosts(category, ITEMS_PER_PAGE, (page - 1) * ITEMS_PER_PAGE);
			return Listing(posts, count, page, Views.Category, new CategoryListingViewModel
			{
				Category = category,
				RssUrl = Url.ActionAbsolute(MVC.Feed.BlogCategory(category.Slug, category.ParentSlug))
			});
		}

		/// <summary>
		/// Viewing a listing of all posts tagged by a particular tag
		/// </summary>
		/// <param name="slug">Tag slug</param>
		/// <param name="page">Page number to view</param>
		/// <returns>Posts tagged with this tag</returns>
		public virtual ActionResult Tag(string slug, int page = 1)
		{
			TagModel tag;
			try
			{
				tag = _blogRepository.GetTag(slug);
			}
			catch (EntityNotFoundException)
			{
				// Throw a 404 if the category doesn't exist
				return HttpNotFound(string.Format("Tag '{0}' not found.", slug));
			}

			var count = _blogRepository.PublishedCount(tag);
			var posts = _blogRepository.LatestPosts(tag, ITEMS_PER_PAGE, (page - 1) * ITEMS_PER_PAGE);
			return Listing(posts, count, page, Views.Tag, new TagListingViewModel { Tag = tag });
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
			catch (EntityNotFoundException)
			{
				// Throw a 404 if the post doesn't exist
				return HttpNotFound(string.Format("Blog post '{0}' not found.", slug));
			}

			// Check the URL was actually correct (year and month), redirect if not.
			if (year != post.Date.Year || month != post.Date.Month)
			{
				return RedirectPermanent(Url.BlogPost(post));
			}

			// Set last-modified date based on the date of the post
			Response.Cache.SetLastModified(post.Date);

			return View(new PostViewModel
			{
				Post = post,
				PostCategories = _blogRepository.CategoriesForPost(post),
				PostTags = _blogRepository.TagsForPost(post),
				ShortUrl = Url.Action(MVC.Blog.ShortUrl(_urlShortener.Shorten(post)), "http"),
				SocialNetworks = GetSocialNetworks(post),
				Comments = _commentRepository.GetCommentsTree(post)
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

			return RedirectPermanent(Url.BlogPost(post));
		}

		/// <summary>
		/// Gets the short URL for this blog post
		/// </summary>
		/// <param name="post">Blog post</param>
		/// <returns>The short URL</returns>
		private string ShortUrl(PostSummaryModel post)
		{
			return Url.Action(MVC.Blog.ShortUrl(_urlShortener.Shorten(post)), "http");
		}

		/// <summary>
		/// Gets the social network URLs and share counts for the specified post
		/// </summary>
		/// <param name="post">Post to get statistics on</param>
		/// <returns>Social network URLs and share counts for the post</returns>
		private IEnumerable<PostSocialNetworkModel> GetSocialNetworks(PostSummaryModel post)
		{
			var shareCounts = post.ShareCounts ?? new Dictionary<string, int>();
			var socialNetworks = _socialManager.ShareUrls(post, Url.BlogPostAbsolute(post), ShortUrl(post));

			return socialNetworks.Select(x => new PostSocialNetworkModel
			{
				SocialNetwork = x.Key,
				Url = x.Value,
				Count = shareCounts.ContainsKey(x.Key.Id) ? shareCounts[x.Key.Id] : 0
			});
		}
	}
}
