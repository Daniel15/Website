using System.Collections.Generic;
using Daniel15.Web.Models.Blog;
using Daniel15.Web.Models.Projects;

namespace Daniel15.Web.ViewModels.Feed
{
	public class SitemapViewModel
	{
		public IList<PostModel> Posts { get; set; }
		public IList<CategoryModel> Categories { get; set; }
		public IList<TagModel> Tags { get; set; }
		public IList<ProjectModel> Projects { get; set; }
	}
}