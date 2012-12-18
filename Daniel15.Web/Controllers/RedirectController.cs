using System.Web.Mvc;
using System.Web.Optimization;

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
	        return Redirect(Styles.Url("~/bundles/main.css").ToString());
        }

		/// <summary>
		/// Redirects to the latest JavaScript file
		/// </summary>
		/// <returns></returns>
		public virtual ActionResult Js()
		{
			return Redirect(Scripts.Url("~/bundles/main.js").ToString());
		}
    }
}
