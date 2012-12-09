using System.Web.Mvc;
using Daniel15.Web.Repositories;
using Daniel15.Web.ViewModels.Blog;

namespace Daniel15.Web.Controllers
{
	public partial class BlogPartialsController : Controller
	{
		private readonly IBlogRepository _blogRepository;

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
			return PartialView(new SidebarViewModel
			{
				Counts = _blogRepository.MonthCounts()
			});
		}
	}
}
