using System.Collections.Generic;
using Daniel15.Web.Models;

namespace Daniel15.Web.ViewModels.Site
{
	/// <summary>
	/// Data required to render the home page
	/// </summary>
	public class IndexViewModel : ViewModelBase
	{
		public List<BlogPostSummaryModel> LatestPosts { get; set; }
	}
}