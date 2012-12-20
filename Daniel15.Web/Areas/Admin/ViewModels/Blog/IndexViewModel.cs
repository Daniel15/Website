using Daniel15.Web.ViewModels;

namespace Daniel15.Web.Areas.Admin.ViewModels.Blog
{
	public class IndexViewModel : ViewModelBase
	{
		public int PublishedPosts { get; set; }
		public int UnpublishedPosts { get; set; }
	}
}