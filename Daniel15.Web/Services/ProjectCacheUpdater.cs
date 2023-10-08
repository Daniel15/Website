using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Daniel15.Web.Services;
using Daniel15.Web.Services.CodeRepositories;
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

		private readonly HttpClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectCacheUpdater" /> class.
		/// </summary>
		/// <param name="markdown">The markdown processor</param>
		/// <param name="projectRepository">The project repository.</param>
		/// <param name="repositoryManager">The code repository manager</param>
		/// <param name="client">HTTP client for loading readmes and other network content</param>
		public ProjectCacheUpdater(IMarkdownProcessor markdown, IProjectRepository projectRepository, ICodeRepositoryManager repositoryManager, HttpClient client)
		{
			_markdown = markdown;
			_projectRepository = projectRepository;
			_repositoryManager = repositoryManager;
			_client = client;
		}

		/// <summary>
		/// Update cached details for the specified project
		/// </summary>
		/// <param name="project">Project to update</param>
		public async Task UpdateProjectAsync(ProjectModel project)
		{
			var updates = await Task.WhenAll(
				UpdateReadmeAsync(project),
				UpdateRepositoryAsync(project)
			);

			if (updates.Any(x => x))
			{
				_projectRepository.Save(project);
			}
		}

		/// <summary>
		/// Updates the readme for the specified project
		/// </summary>
		/// <param name="project">The project to update</param>
		/// <returns><c>true</c> if any data was updated</returns>
		private async Task<bool> UpdateReadmeAsync(ProjectModel project)
		{
			if (string.IsNullOrWhiteSpace(project.ReadmeUrl))
				return false;

			var readmeSource = await _client.GetStringAsync(project.ReadmeUrl);
			project.Readme = _markdown.Parse(readmeSource);
			return true;
		}

		/// <summary>
		/// Updates the repository information for the specified project
		/// </summary>
		/// <param name="project">The project to update</param>
		private async Task<bool> UpdateRepositoryAsync(ProjectModel project)
		{
			if (string.IsNullOrWhiteSpace(project.RepositoryUrl))
				return false;

			var uri = new Uri(project.RepositoryUrl);
			var info = await _repositoryManager.GetRepositoryInfoAsync(uri);
			// TODO: Use AutoMapper here
			project.Created = info.Created;
			project.Updated = info.Updated;
			project.Forks = info.Forks;
			project.Watchers = info.Watchers;
			project.OpenIssues = info.OpenIssues;
			return true;
		}
	}
}
