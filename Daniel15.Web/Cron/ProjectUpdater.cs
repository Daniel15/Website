using Coravel.Invocable;
using Daniel15.Web.Models.Projects;
using Daniel15.Web.Repositories;
using Daniel15.Web.Services;
using Daniel15.Web.Services.CodeRepositories;
using Markdig;

namespace Daniel15.Cron
{
	/// <summary>
	/// Cron task to handle updating cached information for projects
	/// </summary>
	public class ProjectUpdater : IInvocable
	{
		private readonly IProjectRepository _projectRepository;
		private readonly ILogger<ProjectUpdater> _logger;
		private readonly IMarkdownProcessor _markdown;
		private readonly HttpClient _client;
		private readonly ICodeRepositoryManager _repositoryManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectUpdater" /> class.
		/// </summary>
		public ProjectUpdater(
			IProjectRepository projectRepository,
			ILogger<ProjectUpdater> logger,
			IMarkdownProcessor markdown,
			HttpClient client,
			ICodeRepositoryManager repositoryManager
		)
		{
			_projectRepository = projectRepository;
			_logger = logger;
			_markdown = markdown;
			_client = client;
			_repositoryManager = repositoryManager;
		}

		/// <summary>
		/// Runs the project updater task. Updates cached information for all projects in the database.
		/// </summary>
		public async Task Invoke()
		{
			var projects = _projectRepository.All();
			foreach (var project in projects)
			{
				_logger.LogInformation("Updating '{0}'... ", project.Name);
				// TODO: This can be parallelised
				await UpdateProjectAsync(project);
				_logger.LogInformation("Done.");
			}
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
