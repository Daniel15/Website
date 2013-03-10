using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.UI;
using Daniel15.Data.Repositories;
using Daniel15.Web.Models.Home;
using Daniel15.Web.ViewModels;
using Daniel15.Web.ViewModels.Shared;
using Daniel15.Web.ViewModels.Site;
using System.Linq;

namespace Daniel15.Web.Controllers
{
	/// <summary>
	/// Controller for the home page as well as a few auxiliary pages.
	/// </summary>
	public partial class SiteController : Controller
	{
		/// <summary>
		/// One hour in seconds.
		/// </summary>
		private const int ONE_HOUR = 3600;

		private readonly IBlogRepository _blogRepository;
		private readonly IProjectRepository _projectRepository;
		private readonly IMicroblogRepository _microblogRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="SiteController" /> class.
		/// </summary>
		/// <param name="blogRepository">The blog post repository.</param>
		/// <param name="projectRepository">The project repository</param>
		/// <param name="microblogRepository">The microblog (Tumblr) repository.</param>
		public SiteController(IBlogRepository blogRepository, IProjectRepository projectRepository, IMicroblogRepository microblogRepository)
		{
			_blogRepository = blogRepository;
			_projectRepository = projectRepository;
			_microblogRepository = microblogRepository;
		}

		/// <summary>
		/// Home page of the site :-)
		/// </summary>
		/// <returns></returns>
		[OutputCache(Location = OutputCacheLocation.Downstream, Duration = ONE_HOUR)]
		public virtual ActionResult Index()
		{
			return View(Views.Index, new IndexViewModel
			{
				LatestPosts = _blogRepository.LatestPostsSummary()
			});
		}

		/// <summary>
		/// A list of all the projects I've worked on the past
		/// </summary>
		/// <returns></returns>
		public virtual ActionResult Projects()
		{
			var projects = _projectRepository.All();

			return View(Views.Projects, new ProjectsViewModel
			{
				CurrentProjects = projects.Where(x => x.IsCurrent).ToList(),
				PreviousProjects = projects.Where(x => !x.IsCurrent).ToList(),
				PrimaryTechnologies = _projectRepository.PrimaryTechnologies(),
			});
		}

		/// <summary>
		/// Renders the Google search box.
		/// </summary>
		/// <returns></returns>
		public virtual ActionResult Search()
		{
			return View(Views.Search, new ViewModelBase());
		}

		/// <summary>
		/// A feed of all the stuff I've done on the interwebs.
		/// </summary>
		/// <returns></returns>
		public virtual ActionResult SocialFeed()
		{
			// Currently just proxies to the PHP page - This needs to be rewritten in C#
			var content = new WebClient().DownloadString("http://dan.cx/socialfeed/loadhtml.php?" + Request.QueryString);
			return View(Views.SocialFeed, new SocialFeedViewModel { Content = content });
		}

		/// <summary>
		/// A list of recent Tumblr posts
		/// </summary>
		/// <returns></returns>
		[OutputCache(Duration = 86400)]
		public virtual ActionResult TumblrPosts()
		{
			var posts = _microblogRepository.LatestPosts();
			return PartialView(Views._TumblrPosts, posts);
		}

		/// <summary>
		/// The page that is displayed when a File Not Found (404) error occurs.
		/// </summary>
		/// <returns></returns>
		public virtual ActionResult FileNotFound()
		{
			// Nginx will handle setting these as long as fastcgi_intercept_errors is on
			//Response.StatusCode = (int) HttpStatusCode.NotFound;
			//Response.TrySkipIisCustomErrors = true;

			return View(Views.FileNotFound, new ViewModelBase());
		}

		/// <summary>
		/// The page that is displayed when an internal server error occurs.
		/// </summary>
		/// <returns>The error page</returns>
		public virtual ActionResult Error()
		{
			Response.StatusCode = (int) HttpStatusCode.InternalServerError;
			Response.TrySkipIisCustomErrors = true;
			return View(MVC.Shared.Views.ErrorWithLayout, new ErrorViewModel());
		}

		/// <summary>
		/// Page used by Pingdom to determine that the site is running. The index page is not used
		/// as this could be cached by a frontend cache (and hence be accessible while the ASP.NET
		/// MVC part of the site is down)
		/// </summary>
		/// <returns></returns>
		[OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
		public virtual ActionResult Alive()
		{
			return Content("Site is alive and running :)");
		}
	}
}
