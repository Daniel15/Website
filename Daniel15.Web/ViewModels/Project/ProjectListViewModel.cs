using System.Collections.Generic;
using Daniel15.Web.Models.Projects;

namespace Daniel15.Web.ViewModels.Project
{
	public class ProjectListViewModel
	{
		public IEnumerable<ProjectModel> Projects { get; set; }
		public IDictionary<string, ProjectTechnologyModel> Technologies { get; set; }
	}
}
