using System;
using System.Net;
using System.Text;
using System.Web.Helpers;
using Daniel15.BusinessLayer.Services;
using Daniel15.Data.Entities.Projects;
using Daniel15.Data.Repositories;
using ServiceStack.Text;

namespace Daniel15.BusinessLayer
{
	/// <summary>
	/// Handles updating cached information for a project
	/// </summary>
	public class ProjectCacheUpdater : IProjectCacheUpdater
	{
		/// <summary>
		/// URL to the Github API endpoint to get repository details
		/// </summary>
		private const string GITHUB_API_URL = "https://api.github.com/repos/{0}/{1}";

		/// <summary>
		/// Markdown processor used for README files
		/// </summary>
		private readonly IMarkdownProcessor _markdown;
		/// <summary>
		/// The project repository
		/// </summary>
		private readonly IProjectRepository _projectRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectCacheUpdater" /> class.
		/// </summary>
		/// <param name="markdown">The markdown processor</param>
		/// <param name="projectRepository">The project repository.</param>
		public ProjectCacheUpdater(IMarkdownProcessor markdown, IProjectRepository projectRepository)
		{
			_markdown = markdown;
			_projectRepository = projectRepository;
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
			if (uri.Scheme.Equals("git", StringComparison.InvariantCultureIgnoreCase) && uri.Host.Equals("github.com", StringComparison.InvariantCultureIgnoreCase))
			{
				UpdateGithubRepository(project, uri);
			}
			else
			{
				throw new Exception("Unknown repository host: " + project.RepositoryUrl);
			}
			return true;
		}

		/// <summary>
		/// Updates the Github repository information for the specified project
		/// </summary>
		/// <param name="project">Project to update</param>
		/// <param name="repositoryUri">Github repository URI</param>
		private void UpdateGithubRepository(ProjectModel project, Uri repositoryUri)
		{
			// Split repository URI into username and repository
			// Also trim the ".git" off the end.
			var uriPieces = repositoryUri.AbsolutePath.TrimStart('/').Replace(".git", string.Empty).Split('/');
			var user = uriPieces[0];
			var repos = uriPieces[1];

			// Get repository details via API
			var apiUrl = string.Format(GITHUB_API_URL, user, repos);
			using (var client = new WebClient())
			{
				client.Encoding = Encoding.UTF8;
				var responseText = client.DownloadString(apiUrl);
				var response = Json.Decode(responseText);
				project.Created = DateTime.Parse(response.created_at);
				project.Updated = DateTime.Parse(response.updated_at);
			}
		}
	}
}
