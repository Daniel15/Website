using System.Web.Mvc;
using Daniel15.Web.ViewModels.Site;

namespace Daniel15.Web.Controllers
{
	/// <summary>
	/// Controller for the home page as well as a few auxiliary pages.
	/// </summary>
	public partial class SiteController : Controller
    {
		public virtual ActionResult Index()
        {
            return View(new IndexViewModel());
        }
    }
}
