using System.Linq;
using Daniel15.Data.Repositories;
using Daniel15.Web.ViewModels.Blog;
using Microsoft.AspNet.Mvc;

namespace Daniel15.Web.ViewComponents
{
    public class BlogSidebarViewComponent : ViewComponent
    {
	    private readonly IBlogRepository _blogRepository;

	    public BlogSidebarViewComponent(IBlogRepository blogRepository)
	    {
		    _blogRepository = blogRepository;
	    }

	    public IViewComponentResult Invoke()
	    {
			// Group categories based on parent ID - Root level categories will be grouped with parent ID = 0
			var groupedCategories = _blogRepository.CategoriesInUse()
				.GroupBy(x => x.ParentId)
				.ToDictionary(x => x.Key ?? 0, x => x.ToList());

			return View("../../_Sidebar", new SidebarViewModel
			{
				Counts = _blogRepository.MonthCounts(),
				Categories = groupedCategories
			});
		}
    }
}
