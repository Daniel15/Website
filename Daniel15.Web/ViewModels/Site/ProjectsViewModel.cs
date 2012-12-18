using System.Collections.Generic;
using Daniel15.Web.Models.Home;

namespace Daniel15.Web.ViewModels.Site
{
	/// <summary>
	/// Data required to render the projects page
	/// </summary>
	public class ProjectsViewModel : ViewModelBase
	{
		public IList<ProjectModel> CurrentProjects { get; set; }
		public IList<ProjectModel> PreviousProjects { get; set; }
		public IList<ProjectTechnologyModel> PrimaryTechnologies { get; set; }
	}
}