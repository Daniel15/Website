using System.Collections.Generic;
using Daniel15.Web.Models;

namespace Daniel15.Web.ViewModels.Blog
{
	public class IndexViewModel : ViewModelBase
	{
		/// <summary>
		/// Posts to display on the current blog page
		/// </summary>
		public IList<BlogPostModel> Posts { get; set; }

		/// <summary>
		/// Total number of posts that are available
		/// </summary>
		public int TotalCount { get; set; }
	}
}