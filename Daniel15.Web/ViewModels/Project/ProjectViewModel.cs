using System.Collections.Generic;
using Daniel15.Web.Models.Projects;

namespace Daniel15.Web.ViewModels.Project
{
	public class ProjectViewModel : ViewModelBase
	{
		public ProjectModel Project { get; set; }
		public IEnumerable<ProjectTechnologyModel> Technologies { get; set; }
	}
}
