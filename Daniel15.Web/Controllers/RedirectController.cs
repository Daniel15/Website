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
    }
}
