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
		public virtual ActionResult Index()
        {
			var posts = _blogRepository.LatestPosts();

			//throw new NotImplementedException();
			return View(new IndexViewModel());
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
