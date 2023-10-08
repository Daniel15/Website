using System;
using System.Threading;
using Daniel15.Data;
using Daniel15.Data.Entities.Blog;
using Daniel15.Data.Repositories;
using Daniel15.Data.Zurl;
using Daniel15.Web.Extensions;
using Daniel15.Web.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Daniel15.Web.Controllers
{
    public class ShortUrlController : Controller
    {
		private const int MAX_SHORT_URL_LENGTH = 4;

	    private readonly IUrlShortener _urlShortener;
	    private readonly IBlogRepository _blogRepository;
	    private readonly IUrlRepository _urlRepository;

	    public ShortUrlController(
			IUrlShortener urlShortener, 
			IBlogRepository blogRepository, 
			IUrlRepository urlRepository
		)
	    {
		    _urlShortener = urlShortener;
		    _blogRepository = blogRepository;
		    _urlRepository = urlRepository;
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

			var ip = HttpContext.Connection.RemoteIpAddress.ToString();
			var userAgent = Request.Headers["User-Agent"].ToString();
			var referrer = Request.Headers["Referer"].ToString();
			BackgroundJob.Enqueue<IShortUrlLogger>(logger =>
				logger.LogHitAsync(
					shortenedUrl.Id,
					ip,
					userAgent,
					referrer
				)
			);

			return Redirect(shortenedUrl.Url);
		}
	}
}
