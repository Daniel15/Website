using System.Net.Http;
using System.Threading.Tasks;
using Daniel15.Data.Repositories;
using Daniel15.Shared.Extensions;
using Daniel15.Web.ViewModels;
using Daniel15.Web.ViewModels.Site;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Daniel15.Web.Controllers
{
	/// <summary>
	/// Controller for the home page as well as a few auxiliary pages.
	/// </summary>
	[Route("[action].htm")]
	public partial class SiteController : Controller
	{
		/// <summary>
		/// One hour in seconds.
		/// </summary>
		private const int ONE_HOUR = 3600;

		private readonly IBlogRepository _blogRepository;
		private readonly HttpClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="SiteController" /> class.
		/// </summary>
		/// <param name="blogRepository">The blog post repository.</param>
		public SiteController(IBlogRepository blogRepository, HttpClient client)
		{
			_blogRepository = blogRepository;
			_client = client;
		}

		/// <summary>
		/// Home page of the site :-)
		/// </summary>
		[Route("~/")]
		[ResponseCache(Location = ResponseCacheLocation.Any, Duration = ONE_HOUR)]
		public virtual ActionResult Index()
		{
			return View(new IndexViewModel
			{
				LatestPosts = _blogRepository.LatestPosts()
			});
		}

		/// <summary>
		/// Renders the Google search box.
		/// </summary>
		/// <returns></returns>
		public virtual ActionResult Search()
		{
			return View(new ViewModelBase());
		}

		/// <summary>
		/// A feed of all the stuff I've done on the interwebs.
		/// </summary>
		/// <returns></returns>
		// ReSharper disable once InconsistentNaming - Backwards compatibility with old URL
		public virtual async Task<ActionResult> SocialFeed(int count = 25, int? before_date = null)
		{
			// Currently just proxies to the PHP page - This needs to be rewritten in C#
			var url = "http://dan.cx/socialfeed/loadjson.php" + new QueryBuilder
			{
				{"count", count.ToString()},
				{"before_date", before_date.ToString()}
			};

			var response = await _client.GetAsync(url);
			dynamic content = await response.Content.ReadAsJArrayAsync();
			return View("SocialFeed", new SocialFeedViewModel { Data = content });
		}

		/// <summary>
		/// Page used by Pingdom to determine that the site is running. The index page is not used
		/// as this could be cached by a frontend cache (and hence be accessible while the ASP.NET
		/// MVC part of the site is down)
		/// </summary>
		/// <returns></returns>
		[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
		public virtual ActionResult Alive()
		{
			return Content("Site is alive and running :)");
		}

		/// <summary>
		/// Redirects to the new projects page
		/// </summary>
		public virtual ActionResult Projects()
		{
			return RedirectToActionPermanent("Index", "Project");
		}
	}
}
