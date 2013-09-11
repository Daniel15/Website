using System.Collections.Generic;
using Daniel15.Data.Entities.Projects;

namespace Daniel15.Data.Repositories
{
	/// <summary>
	/// Repository for accessing projects I've worked on in the past
	/// </summary>
	public interface IProjectRepository : IRepositoryBase<ProjectModel>
	{
		/// <summary>
		/// Gets a list of the main technologies used to build my sites
		/// </summary>
		/// <returns>A list of technologies</returns>
		IList<ProjectTechnologyModel> Technologies();

		/// <summary>
		/// Gets a project by slug.
		/// </summary>
		/// <param name="slug">The slug.</param>
		/// <returns>The project</returns>
		ProjectModel GetBySlug(string slug);
	}
}
