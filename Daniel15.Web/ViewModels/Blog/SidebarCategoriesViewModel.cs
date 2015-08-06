using System.Collections.Generic;
using Daniel15.Data.Entities.Blog;

namespace Daniel15.Web.ViewModels.Blog
{
    public class SidebarCategoriesViewModel
    {
		public IDictionary<int, List<CategoryModel>> CategoryTree { get; set; }
		public IEnumerable<CategoryModel> Categories { get; set; }
	}
}
