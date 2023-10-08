using System.Collections.Generic;
using Daniel15.Web.Models.Blog;
using Daniel15.Web.ViewModels;

namespace Daniel15.Web.Areas.Admin.ViewModels.Blog
{
	public class PostsViewModel : ViewModelBase
	{
		public IList<PostModel> Posts { get; set; }
	}
}
