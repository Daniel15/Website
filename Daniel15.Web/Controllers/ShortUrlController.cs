using System;
using System.IO;
using System.Threading.Tasks;
using Daniel15.BusinessLayer.Services;
using Daniel15.Data;
using Daniel15.Data.Entities.Blog;
using Daniel15.Data.Repositories;
using Daniel15.Data.Zurl;
using Daniel15.Data.Zurl.Entities;
using Daniel15.Web.Extensions;
using MaxMind.GeoIP2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Daniel15.Web.Controllers
{
    public class ShortUrlController : Controller
    {
		private const int MAX_SHORT_URL_LENGTH = 4;

	    private readonly IUrlShortener _urlShortener;
	    private readonly IBlogRepository _blogRepository;
	    private readonly IUrlRepository _urlRepository;
	    private readonly IHostingEnvironment _hostingEnv;

	    public ShortUrlController(IUrlShortener urlShortener, IBlogRepository blogRepository, IUrlRepository urlRepository, IHostingEnvironment hostingEnv)
	    {
		    _urlShortener = urlShortener;
		    _blogRepository = blogRepository;
		    _urlRepository = urlRepository;
		    _hostingEnv = hostingEnv;
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
	    public async Task<ActionResult> Index(string uri)
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
			if (shortenedUrl != null)
			{
				var hit = CreateHit(shortenedUrl);
				await _urlRepository.AddHitAsync(hit);

				return Content("URL = " + shortenedUrl.Url);
			}

			return NotFound();
		}

	    private ShortenedUrlHit CreateHit(ShortenedUrl shortenedUrl)
	    {
		    var ip = HttpContext.Connection.RemoteIpAddress;
			var hit = new ShortenedUrlHit
		    {
			    Url = shortenedUrl,
			    Date = DateTime.Now,
			    UserAgent = Request.Headers["User-Agent"].ToString(),
			    IpAddress = ip.ToString(),

				// DB should really take nulls for these :(
				ReferrerDomain = "",
				Browser = "",
			    BrowserVersion = "",
				Country = "",
			};

			// Parse user-agent
			if (!string.IsNullOrWhiteSpace(hit.UserAgent))
		    {
			    var parser = UAParser.Parser.GetDefault();
			    var parsedUserAgent = parser.ParseUserAgent(hit.UserAgent);
			    hit.Browser = parsedUserAgent.Family;
			    hit.BrowserVersion = parsedUserAgent.Major + "." + parsedUserAgent.Minor + "." + parsedUserAgent.Patch;
		    }

			// Determine GeoIP country
		    using (var geoIp = new DatabaseReader(Path.Combine(_hostingEnv.ContentRootPath, "GeoLite2-Country.mmdb")))
		    {
			    if (geoIp.TryCountry(ip, out var geoIpResponse))
			    {
				    hit.Country = geoIpResponse.Country.IsoCode;
			    }
		    }

			// Parse domain from referrer
		    if (Uri.TryCreate(Request.Headers["Referer"].ToString(), UriKind.Absolute, out var referrerUri))
		    {
			    hit.Referrer = referrerUri;
			    hit.ReferrerDomain = referrerUri.Host.Replace("www.", string.Empty);
		    }

		    return hit;
	    }
	}
}
