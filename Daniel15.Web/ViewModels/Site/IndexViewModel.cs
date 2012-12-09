using System.Collections.Generic;
using Daniel15.Web.Models;

namespace Daniel15.Web.ViewModels.Site
{
	public class IndexViewModel : ViewModelBase
	{
		public List<BlogPostSummaryModel> LatestPosts { get; set; }
	}
}