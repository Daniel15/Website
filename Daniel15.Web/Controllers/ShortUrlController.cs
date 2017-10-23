using Daniel15.BusinessLayer.Services;
using Daniel15.Data;
using Daniel15.Data.Entities.Blog;
using Daniel15.Data.Repositories;
using Daniel15.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Daniel15.Web.Controllers
{
    public class ShortUrlController : Controller
    {
	    private readonly IUrlShortener _urlShortener;
	    private readonly IBlogRepository _blogRepository;

	    public ShortUrlController(IUrlShortener urlShortener, IBlogRepository blogRepository)
	    {
		    _urlShortener = urlShortener;
		    _blogRepository = blogRepository;
	    }


	    /// <summary>
	    /// Blog short URL redirect - Looks up a short URL and redirects to the post
	    /// </summary>
	    /// <param name="alias">URL alias</param>
	    /// <returns>Redirect to correct post</returns>
	    [Route(@"B{alias:regex(^[[0-9A-Za-z\-_]]+$)}", Order = 998)]
	    public virtual ActionResult Blog(string alias)
	    {
		    var id = _urlShortener.Extend(alias);
		    PostModel post;
		    try
		    {
			    post = _blogRepository.Get(id);
		    }
		    catch (EntityNotFoundException)
		    {
			    return NotFound();
		    }

		    return RedirectPermanent(Url.BlogPost(post));
	    }
	}
}
