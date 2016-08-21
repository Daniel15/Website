using Microsoft.AspNetCore.Mvc;

namespace Daniel15.Web.Controllers
{
	/// <summary>
	/// Handles various redirects
	/// </summary>
	public partial class RedirectController : Controller
    {
		/// <summary>
		/// Redirects from an old blog URL (/blog/year/month/day/slug) to a new one
		/// </summary>
		[Route("blog/{year:int:length(4)}/{month:int:length(2)}/{day:int:length(2)}/{slug}", Order = 98)]
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
		[Route("blog/{*uri}", Order = 99)]
		public virtual ActionResult BlogUri(string uri)
		{
			var redirect = "~/" + uri;
			if (Request.Query != null && Request.Query.Count > 0)
				redirect += Request.QueryString;

			return RedirectPermanent(redirect);
		}
    }
}
