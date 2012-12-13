using System;
using System.Web.Mvc;
using Daniel15.Web.Models;
using Daniel15.Web.Models.Blog;
using Daniel15.Web.Repositories;
using Daniel15.Web.ViewModels.Blog;
using ServiceStack.Common.Web;
using Daniel15.Web.Extensions;

namespace Daniel15.Web.Controllers
{
	/// <summary>
	/// Controller for the main blog pages
	/// </summary>
	public partial class BlogController : Controller
    {
	    private readonly IBlogRepository _blogRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="BlogController" /> class.
		/// </summary>
		/// <param name="blogRepository">The blog post repository.</param>
		public BlogController(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}

		/// <summary>
		/// Index page of the blog
		/// </summary>
		public virtual ActionResult Index(int page = 1)
        {
			// TODO: need to use page number
			// TODO: Load based on page number
			// TODO: Set title based on page number -  ($page_number != 1 ? 'Page ' . $page_number . ' &mdash; ' : '') . $this->config->name
			// TODO: Business layer class for getting posts - Need to join to maincategory
			// TODO: Pagination
			// TODO: Social media sharing URLs
			return View(new IndexViewModel
			{
				Posts = _blogRepository.LatestPosts(),
				TotalCount = _blogRepository.Count()
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
			});
		}

		public virtual ActionResult Category(string slug)
		{
			throw new NotImplementedException();
		}
    }
}
