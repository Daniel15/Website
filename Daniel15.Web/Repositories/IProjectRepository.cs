using System.Collections.Generic;
using Daniel15.Web.Models.Home;

namespace Daniel15.Web.Repositories
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
		IList<ProjectTechnologyModel> PrimaryTechnologies();
	}
}
