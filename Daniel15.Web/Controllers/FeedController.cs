using System.Web.Mvc;
using Daniel15.Web.Repositories;
using Daniel15.Web.ViewModels.Feed;

namespace Daniel15.Web.Controllers
{
	/// <summary>
	/// Handles rendering various feeds
	/// </summary>
	public class FeedController : Controller
	{
		private readonly IBlogRepository _blogRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="FeedController" /> class.
		/// </summary>
		/// <param name="blogRepository">The blog repository.</param>
		public FeedController(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}

		/// <summary>
		/// Gets a sitemap for the website
		/// </summary>
		/// <returns>Sitemap XML</returns>
		public ActionResult Sitemap()
		{
			Response.ContentType = "text/xml";
			return View(new SitemapViewModel
			{
				Posts = _blogRepository.LatestPostsSummary(10000), // Should be big enough, lols
				Categories = _blogRepository.Categories(),
				Tags = _blogRepository.Tags()
			});
		}
	}
}
