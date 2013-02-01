using System.Collections.Generic;
using Daniel15.Data.Entities.Blog;

namespace Daniel15.Web.ViewModels.Feed
{
	public class SitemapViewModel
	{
		public IList<PostSummaryModel> Posts { get; set; }
		public IList<CategoryModel> Categories { get; set; }
		public IList<TagModel> Tags { get; set; } 
	}
}