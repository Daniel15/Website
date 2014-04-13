using System;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Cassette.Views;

namespace Daniel15.Web.Controllers
{
	/// <summary>
	/// Handles various redirects
	/// </summary>
	public partial class RedirectController : Controller
    {
		/// <summary>
		/// Redirects to the latest CSS file
		/// </summary>
		[GET("latest.css")]
		public virtual ActionResult Css()
		{
			return Redirect(Bundles.Url("main.css"));
		}

		/// <summary>
		/// Redirects to the latest JavaScript file
		/// </summary>
		[GET("latest.js")]
		public virtual ActionResult Js()
		{
			return Redirect(Bundles.Url("main.js"));
		}

		/// <summary>
		/// Redirects from an old blog URL (/blog/year/month/day/slug) to a new one
		/// </summary>
		[GET("blog/{year:int:length(4)}/{month:int:length(2)}/{day:int:length(2)}/{slug}", SitePrecedence = -1, ControllerPrecedence = 1)]
		public virtual ActionResult BlogPost(int month, int year, int day, string slug)
		{
			return RedirectToAction("View", "Blog", new { year = year.ToString(), month = month.ToString("00"), slug = slug });
		}

		/// <summary>
		/// Redirect to a blog URL. Used to redirect old /blog/... URLs to remove the "/blog"
		/// Blog pages used to have /blog/ in the URL but this was removed.
		/// Easier to do this redirect here instead of Nginx config as some /blog/ URLs are still valid.
		/// </summary>
		/// <param name="uri">URI to redirect to</param>
		/// <returns>Redirect</returns>
		[GET("blog/{*uri}", SitePrecedence = -1, ControllerPrecedence = 2)]
		public virtual ActionResult BlogUri(string uri)
		{
			var redirect = "~/" + uri;
			if (Request.QueryString != null && Request.QueryString.Count > 0)
				redirect += "?" + Request.QueryString;

			return RedirectPermanent(redirect);
		}
    }
}
