using Daniel15.Data.Entities.Projects;

namespace Daniel15.Web.ViewModels.Project
{
	public class ProjectViewModel : ViewModelBase
	{
		public ProjectModel Project { get; set; }
		public string Content { get; set; }
	}
}