using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.UI;
using Daniel15.Web.Models.Home;
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

		#region Google Talk chat status

		private const string TK = "z01q6amlqaf80ct0iuvnq226055735i723g9omh9525cu7ce7onoqd5vm7quktkdlts0i5d6c8nr113mhh7e06mlu92gmbv1506gcp26fdn3c45cpqlu652rb6ksdsodpjb95s019nqarbqo";
		private const string BADGE_URL = "http://www.google.com/talk/service/badge/Show?tk=" + TK;
		private const string CHAT_URL = "http://www.google.com/talk/service/badge/Start?tk=" + TK;
		private const string CHAT_BASE_URL = "http://www.google.com";
		private static readonly Regex _iconRegex = new Regex("<img id=\"b\" src=\"([^\"]+)\"");
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
