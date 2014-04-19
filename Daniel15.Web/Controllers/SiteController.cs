using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;
using AttributeRouting.Web.Mvc;
using Daniel15.Data.Repositories;
using Daniel15.Shared.Extensions;
using Daniel15.Web.ViewModels;
using Daniel15.Web.ViewModels.Shared;
using Daniel15.Web.ViewModels.Site;

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
		private readonly IMicroblogRepository _microblogRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="SiteController" /> class.
		/// </summary>
		/// <param name="blogRepository">The blog post repository.</param>
		/// <param name="microblogRepository">The microblog (Tumblr) repository.</param>
		public SiteController(IBlogRepository blogRepository, IMicroblogRepository microblogRepository)
		{
			_blogRepository = blogRepository;
			_microblogRepository = microblogRepository;
		}

		/// <summary>
		/// Home page of the site :-)
		/// </summary>
		[GET("")]
		[OutputCache(Location = OutputCacheLocation.Downstream, Duration = ONE_HOUR)]
		public virtual ActionResult Index()
		{
			return View(Views.Index, new IndexViewModel
			{
				LatestPosts = _blogRepository.LatestPostsSummary()
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
		public virtual ActionResult SocialFeed(int count = 25, int? before_date = null)
		{
			// Currently just proxies to the PHP page - This needs to be rewritten in C#
			var url = "http://dan.cx/socialfeed/loadjson.php?" + new Dictionary<string, object>
			{
				{"count", count},
				{"before_date", before_date}
			}.ToQueryString();
			var responseText = new WebClient().DownloadString(url);
			var response = System.Web.Helpers.Json.Decode(responseText);
			return View(Views.SocialFeed, new SocialFeedViewModel { Data = response });
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

		/// <summary>
		/// Redirects to the new projects page
		/// </summary>
		public virtual ActionResult Projects()
		{
			return RedirectToActionPermanent(MVC.Project.Index());
		}
	}
}
