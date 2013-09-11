using System;
using System.Collections.Generic;
using Daniel15.Data.Entities.Projects;
using ServiceStack.OrmLite;
using Daniel15.Data.Extensions;

namespace Daniel15.Data.Repositories.OrmLite
{
	/// <summary>
	/// Blog repository that uses OrmLite as the data access component
	/// </summary>
	public class ProjectRepository : RepositoryBase<ProjectModel>, IProjectRepository
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectRepository" /> class.
		/// </summary>
		/// <param name="connectionFactory">The connection factory.</param>
		public ProjectRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
		{
		}

		/// <summary>
		/// Gets all the project entities in the database
		/// </summary>
		/// <returns>The project entities</returns>
		public override List<ProjectModel> All()
		{
			return Connection.Select<ProjectModel>(query => query.OrderBy(proj => proj.Order));
		}

		/// <summary>
		/// Gets a list of all technologies I've used to build sites
		/// </summary>
		/// <returns>List of technologies</returns>
		public IList<ProjectTechnologyModel> Technologies()
		{
			return Connection.Select<ProjectTechnologyModel>(query => query.OrderBy(tech => tech.Order));
		}

		/// <summary>
		/// Gets a project by its slug
		/// </summary>
		/// <param name="slug"></param>
		/// <returns></returns>
		public ProjectModel GetBySlug(string slug)
		{
			return Connection.FirstOrThrow<ProjectModel>(proj => proj.Slug == slug);
		}
	}
}
