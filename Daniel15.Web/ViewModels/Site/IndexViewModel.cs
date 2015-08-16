using System.Collections.Generic;
using Daniel15.Data.Entities.Blog;

namespace Daniel15.Web.ViewModels.Site
{
	/// <summary>
	/// Data required to render the home page
	/// </summary>
	public class IndexViewModel : ViewModelBase
	{
		public List<PostModel> LatestPosts { get; set; }
	}
}