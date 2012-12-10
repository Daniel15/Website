using System;
using System.Web.Mvc;
using Daniel15.Web.Repositories;
using Daniel15.Web.ViewModels.Blog;

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
			throw new NotImplementedException("TODO: View " + slug);
		}
    }
}
