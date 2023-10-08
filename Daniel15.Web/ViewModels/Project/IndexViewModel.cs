using System.Collections.Generic;
using Daniel15.Web.Models.Projects;

namespace Daniel15.Web.ViewModels.Project
{
	/// <summary>
	/// Data required to render the projects page
	/// </summary>
	public class IndexViewModel : ViewModelBase
	{
		public IList<ProjectModel> CurrentProjects { get; set; }
		public IList<ProjectModel> PreviousProjects { get; set; }
		public IList<ProjectTechnologyModel> PrimaryTechnologies { get; set; }
		public IDictionary<string, ProjectTechnologyModel> Technologies { get; set; }
	}
}
