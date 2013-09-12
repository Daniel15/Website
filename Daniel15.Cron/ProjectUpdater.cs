using System;
using Daniel15.BusinessLayer;
using Daniel15.Data.Repositories;

namespace Daniel15.Cron
{
	/// <summary>
	/// Cron task to handle updating cached information for projects
	/// </summary>
	public class ProjectUpdater
	{
		private readonly IProjectCacheUpdater _projectUpdater;
		private readonly IProjectRepository _projectRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectUpdater" /> class.
		/// </summary>
		/// <param name="projectUpdater">The project updater.</param>
		/// <param name="projectRepository">The project repository.</param>
		public ProjectUpdater(IProjectCacheUpdater projectUpdater, IProjectRepository projectRepository)
		{
			_projectUpdater = projectUpdater;
			_projectRepository = projectRepository;
		}

		/// <summary>
		/// Runs the project updater task. Updates cached information for all projects in the database.
		/// </summary>
		public void Run()
		{
			var projects = _projectRepository.All();
			foreach (var project in projects)
			{
				Console.Write("Updating '{0}'... ", project.Name);
				_projectUpdater.UpdateProject(project);
				Console.WriteLine("Done.");
			}
		}
	}
}
