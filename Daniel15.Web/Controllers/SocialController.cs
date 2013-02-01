using System.Web.Mvc;
using System.Web.UI;
using Daniel15.Data.Repositories;
using Daniel15.Web.Services;
using Daniel15.Web.Services.Social;
using Daniel15.Web.Extensions;
using System.Linq;

namespace Daniel15.Web.Controllers
{
	/// <summary>
	/// Handles auxiliary stuff relating to sharing on social networks
	/// </summary>
	public partial class SocialController : Controller
	{
		private readonly ISocialManager _socialManager;
		private readonly IBlogRepository _blogRepository;
		private readonly IUrlShortener _urlShortener;

		/// <summary>
		/// Initializes a new instance of the <see cref="SocialController" /> class.
		/// </summary>
		/// <param name="socialManager">The social manager.</param>
		/// <param name="blogRepository">The blog repository.</param>
		/// <param name="urlShortener">The URL shortener.</param>
		public SocialController(ISocialManager socialManager, IBlogRepository blogRepository, IUrlShortener urlShortener)
		{
			_socialManager = socialManager;
			_blogRepository = blogRepository;
			_urlShortener = urlShortener;
		}

		/// <summary>
		/// Gets the number of times this post has been shared on all social networks.
		/// </summary>
		/// <param name="slug">The post's slug.</param>
		/// <returns>JSON data</returns>
		[OutputCache(Duration = 1800, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "slug")]
		public virtual ActionResult PostShareCount(string slug)
		{
			var post = _blogRepository.GetSummaryBySlug(slug);
			var url = Url.BlogPostAbsolute(post);
			var shortUrl = Url.Action(MVC.Blog.ShortUrl(_urlShortener.Shorten(post)), "http");
			var counts = _socialManager.ShareCounts(post, url, shortUrl);
			return Json(counts.ToDictionary(x => x.Key.Id, x => x.Value), JsonRequestBehavior.AllowGet);
		}
	}
}
