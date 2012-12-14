using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Daniel15.Web.Models.Blog;
using Daniel15.Web.Repositories;
using Daniel15.Web.Services;
using Daniel15.Web.ViewModels.Blog;
using Daniel15.Web.Extensions;

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

		private readonly IBlogRepository _blogRepository;
		private readonly IUrlShortener _urlShortener;

		/// <summary>
		/// Initializes a new instance of the <see cref="BlogController" /> class.
		/// </summary>
		/// <param name="blogRepository">The blog post repository.</param>
		/// <param name="urlShortener">The URL shortener</param>
		public BlogController(IBlogRepository blogRepository, IUrlShortener urlShortener)
		{
			_blogRepository = blogRepository;
			_urlShortener = urlShortener;
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
		private ActionResult Listing(IList<PostModel> posts, int count, int page, string viewName = null, ListingViewModel viewModel = null)
		{
			if (viewName == null)
				viewName = Views.Index;

			if (viewModel == null)
				viewModel = new ListingViewModel();

			var pages = (int)Math.Ceiling((double)count / ITEMS_PER_PAGE);

			viewModel.Posts = posts;
			viewModel.TotalCount = count;
			viewModel.Page = page;
			viewModel.TotalPages = pages;
			viewModel.UrlShortener = ShortUrl;
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

			return View(new PostViewModel
			{
				Post = post,
				PostCategories = _blogRepository.CategoriesForPost(post),
				ShortUrl = Url.Action(MVC.Blog.ShortUrl(_urlShortener.Shorten(post)), "http")
			});
		}

		/// <summary>
		/// Short URL redirect - Looks up a short URL and redirects to the post
		/// </summary>
		/// <param name="alias">URL alias</param>
		/// <returns>Redirect to correct post</returns>
		public virtual ActionResult ShortUrl(string alias)
		{
			throw new NotImplementedException();
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
