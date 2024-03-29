using Coravel.Queuing.Interfaces;
using Daniel15.Web.Exceptions;
using Daniel15.Web.Models.Blog;
using Daniel15.Web.Repositories;
using Daniel15.Web.Zurl;
using Daniel15.Web.Extensions;
using Daniel15.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Daniel15.Web.Controllers
{
    public class ShortUrlController : Controller
    {
		private const int MAX_SHORT_URL_LENGTH = 4;

	    private readonly IUrlShortener _urlShortener;
	    private readonly IBlogRepository _blogRepository;
	    private readonly IUrlRepository _urlRepository;
	    private readonly IQueue _queue;

	    public ShortUrlController(
			IUrlShortener urlShortener, 
			IBlogRepository blogRepository, 
			IUrlRepository urlRepository,
			IQueue queue
		)
	    {
		    _urlShortener = urlShortener;
		    _blogRepository = blogRepository;
		    _urlRepository = urlRepository;
		    _queue = queue;
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

		/// <summary>
		/// Catch all that handles zURL short URLs
		/// </summary>
		/// <param name="uri">URL alias</param>
		/// <returns>Redirect, or 404 error if nothing matches</returns>
		[Route(@"{uri:regex(^[[0-9A-Za-z\-_]]+$)}", Order = 999)]
		public ActionResult Index(string uri)
		{
			int? id = null;
			if (uri.Length <= MAX_SHORT_URL_LENGTH)
			{
				try
				{
					id = _urlShortener.Extend(uri);
				}
				catch (Exception)
				{
					// Not a short URL by ID... Maybe it's one with a custom alias, or it's not a short URL at all.
				}
			}

			var shortenedUrl = _urlRepository.TryGetByAlias("dl.vc", uri, id);
			if (shortenedUrl == null)
			{
				return NotFound();
			}

			_queue.QueueInvocableWithPayload<ShortUrlLogger, ShortUrlLogger.Input>(new ShortUrlLogger.Input
			{
				Ip = HttpContext.Connection.RemoteIpAddress.ToString(),
				Referrer = Request.Headers["Referer"].ToString(),
				UrlId = shortenedUrl.Id,
				UserAgent = Request.Headers["User-Agent"].ToString(),
			});

			return Redirect(shortenedUrl.Url);
		}
	}
}
