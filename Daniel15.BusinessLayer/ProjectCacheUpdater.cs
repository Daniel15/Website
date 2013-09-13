using System;
using System.Net;
using System.Text;
using Daniel15.BusinessLayer.Services;
using Daniel15.BusinessLayer.Services.CodeRepositories;
using Daniel15.Data.Entities.Projects;
using Daniel15.Data.Repositories;

namespace Daniel15.BusinessLayer
{
	/// <summary>
	/// Handles updating cached information for a project
	/// </summary>
	public class ProjectCacheUpdater : IProjectCacheUpdater
	{
		/// <summary>
		/// Markdown processor used for README files
		/// </summary>
		private readonly IMarkdownProcessor _markdown;
		/// <summary>
		/// The project repository
		/// </summary>
		private readonly IProjectRepository _projectRepository;
		/// <summary>
		/// The code repository manager, to retrieve details on code repositories
		/// </summary>
		private readonly ICodeRepositoryManager _repositoryManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectCacheUpdater" /> class.
		/// </summary>
		/// <param name="markdown">The markdown processor</param>
		/// <param name="projectRepository">The project repository.</param>
		/// <param name="repositoryManager">The code repository manager</param>
		public ProjectCacheUpdater(IMarkdownProcessor markdown, IProjectRepository projectRepository, ICodeRepositoryManager repositoryManager)
		{
			_markdown = markdown;
			_projectRepository = projectRepository;
			_repositoryManager = repositoryManager;
		}

		/// <summary>
		/// Update cached details for the specified project
		/// </summary>
		/// <param name="project">Project to update</param>
		public void UpdateProject(ProjectModel project)
		{
			bool dirty = false;
			dirty |= UpdateReadme(project);
			dirty |= UpdateRepository(project);

			if (dirty)
				_projectRepository.Save(project);
		}

		/// <summary>
		/// Updates the readme for the specified project
		/// </summary>
		/// <param name="project">The project to update</param>
		/// <returns><c>true</c> if any data was updated</returns>
		private bool UpdateReadme(ProjectModel project)
		{
			if (string.IsNullOrWhiteSpace(project.ReadmeUrl))
				return false;

			using (var client = new WebClient())
			{
				client.Encoding = Encoding.UTF8;
				var readmeSource = client.DownloadString(project.ReadmeUrl);
				project.Readme = _markdown.Parse(readmeSource);
				return true;
			}
		}

		/// <summary>
		/// Updates the repository information for the specified project
		/// </summary>
		/// <param name="project">The project to update</param>
		private bool UpdateRepository(ProjectModel project)
		{
			if (string.IsNullOrWhiteSpace(project.RepositoryUrl))
				return false;

			var uri = new Uri(project.RepositoryUrl);
			var info = _repositoryManager.GetRepositoryInfo(uri);
			project.Created = info.Created;
			project.Updated = info.Updated;
			return true;
		}
	}
}
