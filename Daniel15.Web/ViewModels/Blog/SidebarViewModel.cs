using System.Collections.Generic;
using Daniel15.Data.Entities.Blog;

namespace Daniel15.Web.ViewModels.Blog
{
	public class SidebarViewModel
	{
		public IDictionary<int, IDictionary<int, int>> Counts { get; set; }
		public IDictionary<int, List<CategoryModel>> Categories { get; set; }
	}
}