using System.Web.Mvc;
using Daniel15.Data.Repositories;
using Daniel15.Web.ViewModels.Blog;

namespace Daniel15.Web.Controllers
{
	/// <summary>
	/// Handles rendering partial views in the blog
	/// </summary>
	public partial class BlogPartialsController : Controller
	{
		private readonly IBlogRepository _blogRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="BlogPartialsController" /> class.
		/// </summary>
		/// <param name="blogRepository">The blog repository.</param>
		public BlogPartialsController(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}

		/// <summary>
		/// Gets the content of the blog sidebar
		/// </summary>
		/// <returns>Content of the blog sidebar</returns>
		public virtual ActionResult Sidebar()
		{
			return PartialView(Views.Sidebar, new SidebarViewModel
			{
				Counts = _blogRepository.MonthCounts(),
				Categories = _blogRepository.Categories()
			});
		}
	}
}
