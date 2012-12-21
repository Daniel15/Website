using System.Collections.Generic;
using Daniel15.Web.Models.Blog;
using Daniel15.Web.ViewModels;

namespace Daniel15.Web.Areas.Admin.ViewModels.Blog
{
	public class EditViewModel : ViewModelBase
	{
		public PostModel Post { get; set; }

		public IList<CategoryModel> Categories { get; set; }
		public IList<TagModel> Tags { get; set; }
		public IList<int> PostCategoryIds { get; set; }
		public IList<int> PostTagIds { get; set; }
	}
}