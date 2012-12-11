using System.Collections.Generic;
using Daniel15.Web.Models;
using Daniel15.Web.Models.Blog;

namespace Daniel15.Web.ViewModels.Blog
{
	public class IndexViewModel : ViewModelBase
	{
		/// <summary>
		/// Posts to display on the current blog page
		/// </summary>
		public IList<PostModel> Posts { get; set; }

		/// <summary>
		/// Total number of posts that are available
		/// </summary>
		public int TotalCount { get; set; }

		/// <summary>
		/// Current page number
		/// </summary>
		public int Page { get; set; }
	}
}