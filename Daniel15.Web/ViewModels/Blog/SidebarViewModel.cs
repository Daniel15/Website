using System.Collections.Generic;

namespace Daniel15.Web.ViewModels.Blog
{
	public class SidebarViewModel
	{
		public IDictionary<int, IDictionary<int, int>> Counts { get; set; }
	}
}