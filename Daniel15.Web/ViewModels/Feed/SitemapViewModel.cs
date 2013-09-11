using System.Collections.Generic;
using Daniel15.Data.Entities.Blog;
using Daniel15.Data.Entities.Projects;

namespace Daniel15.Web.ViewModels.Feed
{
	public class SitemapViewModel
	{
		public IList<PostSummaryModel> Posts { get; set; }
		public IList<CategoryModel> Categories { get; set; }
		public IList<TagModel> Tags { get; set; }
		public IList<ProjectModel> Projects { get; set; }
	}
}