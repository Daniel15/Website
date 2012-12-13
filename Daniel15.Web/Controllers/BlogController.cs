using System;
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
		/// Index page of the blog
		/// </summary>
		public virtual ActionResult Index(int page = 1)
        {
			Func<PostModel, string> urlShortener = post => Url.Action(MVC.Blog.ShortUrl(_urlShortener.Shorten(post)), "http");

			var count = _blogRepository.Count();
			var pages = (int)Math.Ceiling((double)count / ITEMS_PER_PAGE);

			return View(new IndexViewModel
			{
				Posts = _blogRepository.LatestPosts(ITEMS_PER_PAGE, (page - 1) * ITEMS_PER_PAGE),
				TotalCount = count,
				Page = page,
				TotalPages = pages,
				UrlShortener = urlShortener,
			});
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
		/// Viewing a category listing
		/// </summary>
		/// <param name="slug">Category slug</param>
		/// <param name="page">Page number to view</param>
		/// <returns>Posts in this category</returns>
		public virtual ActionResult Category(string slug, int page = 1)
		{
			throw new NotImplementedException();
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
    }
}
