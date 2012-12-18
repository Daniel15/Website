﻿using System.Collections.Generic;
using Daniel15.Web.Models.Blog;

namespace Daniel15.Web.ViewModels.Blog
{
	public class SidebarViewModel
	{
		public IDictionary<int, IDictionary<int, int>> Counts { get; set; }
		public IList<CategoryModel> Categories { get; set; }
	}
}