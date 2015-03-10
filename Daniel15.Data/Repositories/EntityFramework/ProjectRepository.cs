using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Daniel15.Data.Entities.Projects;

namespace Daniel15.Data.Repositories.EntityFramework
{
	/// <summary>
	/// Project repository that uses Entity Framework as the data access component
	/// </summary>
	public class ProjectRepository : RepositoryBase<ProjectModel>, IProjectRepository
	{
		public ProjectRepository(DatabaseContext context) : base(context) {}

		/// <summary>
		/// Gets the <see cref="DbSet{T}"/> represented by this repository.
		/// </summary>
		protected override DbSet<ProjectModel> Set
		{
			get { return Context.Projects; }
		}

		/// <summary>
		/// Gets all the project entities in the database
		/// </summary>
		/// <returns>The project entities</returns>
		public override List<ProjectModel> All()
		{
			return Context.Projects.OrderBy(proj => proj.Order).ToList();
		}

		/// <summary>
		/// Gets a list of all technologies I've used to build sites
		/// </summary>
		/// <returns>List of technologies</returns>
		public IList<ProjectTechnologyModel> Technologies()
		{
			return Context.Technologies.OrderBy(tech => tech.Order).ToList();
		}

		/// <summary>
		/// Gets a project by its slug
		/// </summary>
		/// <param name="slug"></param>
		/// <returns></returns>
		public ProjectModel GetBySlug(string slug)
		{
			return Context.Projects.First(proj => proj.Slug == slug);
		}
	}
}
