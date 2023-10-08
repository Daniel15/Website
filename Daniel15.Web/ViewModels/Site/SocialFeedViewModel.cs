using Daniel15.Web.Models;

namespace Daniel15.Web.ViewModels.Site
{
	public class SocialFeedViewModel : ViewModelBase
	{
		public IEnumerable<SocialFeedItem> Items { get; set; }

		public bool Partial { get; set; }

		public bool ShowDescription { get; set; }
	}
}
