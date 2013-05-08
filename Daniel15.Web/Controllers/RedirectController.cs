using System;
using System.Web.Mvc;

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
		/// <returns></returns>
		public virtual ActionResult Css()
		{
			return Redirect(Combres.WebExtensions.CombresUrl("main.css"));
		}

		/// <summary>
		/// Redirects to the latest JavaScript file
		/// </summary>
		/// <returns></returns>
		public virtual ActionResult Js()
		{
			return Redirect(Combres.WebExtensions.CombresUrl("main.js"));
		}

		/// <summary>
		/// Redirect to a blog URL. Used to redirect old /blog/... URLs to remove the "/blog"
		/// </summary>
		/// <param name="uri">URI to redirect to</param>
		/// <returns>Redirect</returns>
		public virtual ActionResult BlogUri(string uri)
		{
			var redirect = "~/" + uri;
			if (Request.QueryString != null && Request.QueryString.Count > 0)
				redirect += "?" + Request.QueryString;

			return RedirectPermanent(redirect);
		}
    }
}
