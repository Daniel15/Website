using System.Linq;
using System.Net;
using System.Web.Mvc;
using Daniel15.BusinessLayer.Services;
using Daniel15.Data;
using Daniel15.Data.Entities.Projects;
using Daniel15.Data.Repositories;
using Daniel15.Web.ViewModels.Project;
using IndexViewModel = Daniel15.Web.ViewModels.Project.IndexViewModel;

namespace Daniel15.Web.Controllers
{
	public partial class ProjectController : Controller
    {
		private readonly IProjectRepository _projectRepository;
		private readonly IMarkdownProcessor _markdown;

		public ProjectController(IProjectRepository projectRepository, IMarkdownProcessor markdown)
		{
			_projectRepository = projectRepository;
			_markdown = markdown;
		}

		/// <summary>
		/// A list of all the projects I've worked on the past
		/// </summary>
		/// <returns></returns>
		public virtual ActionResult Index()
		{
			var projects = _projectRepository.All();
			var techs = _projectRepository.Technologies();

			return View(Views.Index, new IndexViewModel
			{
				CurrentProjects = projects.Where(x => x.IsCurrent).ToList(),
				PreviousProjects = projects.Where(x => !x.IsCurrent).ToList(),
				PrimaryTechnologies = techs.Where(x => x.IsPrimary).ToList(),
				Technologies = techs.ToDictionary(x => x.Slug)
			});
		}

		public virtual ActionResult Detail(string slug)
		{
			ProjectModel project;
			try
			{
				project = _projectRepository.GetBySlug(slug);
			}
			catch (EntityNotFoundException)
			{
				// Throw a 404 if the project doesn't exist
				return HttpNotFound(string.Format("Project '{0}' not found.", slug));
			}

			// If there's no readme, just redirect to the project site itself
			if (string.IsNullOrEmpty(project.ReadmeUrl))
			{
				return Redirect(project.Url);
			}

			// Load the readme and display it
			var client = new WebClient();
			var readmeSource = client.DownloadString(project.ReadmeUrl);
			var result = _markdown.Parse(readmeSource);

			return View(new ProjectViewModel
			{
				Project = project,
				Content = result
			});
		}
    }
}
