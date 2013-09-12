using System.Net;
using Daniel15.BusinessLayer.Services;
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
			UpdateReadme(project);
		}

		/// <summary>
		/// Updates the readme for the specified project
		/// </summary>
		/// <param name="project">The project to update</param>
		private void UpdateReadme(ProjectModel project)
		{
			if (string.IsNullOrWhiteSpace(project.ReadmeUrl))
				return;

			using (var client = new WebClient())
			{
				var readmeSource = client.DownloadString(project.ReadmeUrl);
				project.Readme = _markdown.Parse(readmeSource);
				_projectRepository.Save(project);
			}
		}
	}
}
