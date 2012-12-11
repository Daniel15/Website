using System.Collections.Generic;
using Daniel15.Web.Models;
using Daniel15.Web.Models.Blog;

namespace Daniel15.Web.ViewModels.Blog
{
	public class PostViewModel : ViewModelBase
	{
		public PostModel Post { get; set; }
		public IList<CategoryModel> PostCategories { get; set; }
	}
}