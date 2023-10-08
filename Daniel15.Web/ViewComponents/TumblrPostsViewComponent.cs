using Daniel15.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Daniel15.Web.ViewComponents
{
	/// <summary>
	/// Renders a list of recent Tumblr posts
	/// </summary>
	public class TumblrPostsViewComponent : ViewComponent
    {
	    private readonly IMicroblogRepository _microblogRepository;

	    public TumblrPostsViewComponent(IMicroblogRepository microblogRepository)
	    {
		    _microblogRepository = microblogRepository;
	    }

		public IViewComponentResult Invoke()
		{
			var posts = _microblogRepository.LatestPosts();
			return View(posts);
		}
    }
}
