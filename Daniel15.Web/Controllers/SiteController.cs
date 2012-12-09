using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Caching;
using System.Web.Mvc;
using Daniel15.Web.Models.Home;
using Daniel15.Web.Repositories;
using Daniel15.Web.ViewModels.Site;
using Daniel15.Web.Extensions;

namespace Daniel15.Web.Controllers
{
	/// <summary>
	/// Controller for the home page as well as a few auxiliary pages.
	/// </summary>
	public partial class SiteController : Controller
	{
		private readonly IBlogPostRepository _blogPostRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="SiteController" /> class.
		/// </summary>
		/// <param name="blogPostRepository">The blog post repository.</param>
		public SiteController(IBlogPostRepository blogPostRepository)
		{
			_blogPostRepository = blogPostRepository;
		}

		/// <summary>
		/// Home page of the site :-)
		/// </summary>
		/// <returns></returns>
		public virtual ActionResult Index()
        {
			// Load the most recent blog posts
			/*var posts = HttpContext.Cache.GetOrInsert("BlogPosts", DateTime.Now.AddDays(1), Cache.NoSlidingExpiration, () =>
			{
				return _blogPostRepository.LatestPostsSummary();
			});*/

			var posts = _blogPostRepository.LatestPostsSummary();
			
            return View(new IndexViewModel
	        {
		        LatestPosts = posts
	        });
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
