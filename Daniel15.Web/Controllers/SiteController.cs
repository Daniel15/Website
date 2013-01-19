using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml.Linq;
using Daniel15.Web.Infrastructure;
using Daniel15.Web.Models.Home;
using Daniel15.Web.Repositories;
using Daniel15.Web.ViewModels;
using Daniel15.Web.ViewModels.Shared;
using Daniel15.Web.ViewModels.Site;
using Daniel15.Web.Extensions;
using System.Linq;
using ServiceStack.ServiceClient.Web;

namespace Daniel15.Web.Controllers
{
	/// <summary>
	/// Controller for the home page as well as a few auxiliary pages.
	/// </summary>
	public partial class SiteController : Controller
	{
		/// <summary>
		/// How long to cache the recent blog posts for
		/// </summary>
		private static readonly TimeSpan CACHE_POSTS_FOR = new TimeSpan(1, 0, 0);

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
		public virtual ActionResult Index()
		{
			// Load the most recent blog posts
			var posts = HttpContext.Cache.GetOrInsert("LatestPosts", DateTime.Now + CACHE_POSTS_FOR, Cache.NoSlidingExpiration,
			                                          () => _blogRepository.LatestPostsSummary());
			
			return View(Views.Index, new IndexViewModel
			{
				LatestPosts = posts
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
			var content = new WebClient().DownloadString("http://dan.cx/socialfeed/loadhtml.php");
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

		#region Google Talk chat status
		/// <summary>
		/// "tk" parameter from badge URL
		/// </summary>
		private const string TK = "z01q6amlqaf80ct0iuvnq226055735i723g9omh9525cu7ce7onoqd5vm7quktkdlts0i5d6c8nr113mhh7e06mlu92gmbv1506gcp26fdn3c45cpqlu652rb6ksdsodpjb95s019nqarbqo";
		/// <summary>
		/// URL for the badge
		/// </summary>
		private const string BADGE_URL = "http://www.google.com/talk/service/badge/Show?tk=" + TK;
		/// <summary>
		/// URL to initiate chat
		/// </summary>
		private const string CHAT_URL = "http://www.google.com/talk/service/badge/Start?tk=" + TK;
		/// <summary>
		/// Base URL for icons
		/// </summary>
		private const string CHAT_BASE_URL = "http://www.google.com";
		/// <summary>
		/// Retrieves icon from badge data
		/// </summary>
		private static readonly Regex _iconRegex = new Regex("<img id=\"b\" src=\"([^\"]+)\"");
		/// <summary>
		/// Retrieves status from badge data
		/// </summary>
		private static readonly Regex _statusRegex = new Regex("(.+)</div></div></body>");

		/// <summary>
		/// Gets the Google Talk chat status
		/// </summary>
		/// <returns>Chat status data</returns>
		//[OutputCache(Location = OutputCacheLocation.ServerAndClient, Duration = 30)]
		public virtual ActionResult ChatStatus()
		{
			// Download the chat status page
			var badgeData = new WebClient().DownloadString(BADGE_URL);

			// Scrape data from it
			var icon = _iconRegex.Match(badgeData).Groups[1].Value;
			var statusText = _statusRegex.Match(badgeData).Groups[1].Value;

			var model = new ChatStatusModel
			{
				Icon = CHAT_BASE_URL + icon,
				StatusText = statusText
			};

			// Try to guess status based on icon
			if (icon.Contains("online"))
				model.Status = ChatStatusModel.ChatStatus.Online;
			else if (icon.Contains("busy"))
				model.Status = ChatStatusModel.ChatStatus.Busy;
			else
				model.Status = ChatStatusModel.ChatStatus.Offline;

			// Only show chat link if status is "Online"
			if (model.Status == ChatStatusModel.ChatStatus.Online)
				model.ChatUrl = CHAT_URL;

			return Json(model, JsonRequestBehavior.AllowGet);
		}
		#endregion
	}
}
