using Daniel15.Data.Entities.Projects;

namespace Daniel15.BusinessLayer
{
	/// <summary>
	/// Handles updating cached information for a project
	/// </summary>
	public interface IProjectCacheUpdater
	{
		/// <summary>
		/// Update cached details for the specified project
		/// </summary>
		/// <param name="project">Project to update</param>
		void UpdateProject(ProjectModel project);
	}
}
