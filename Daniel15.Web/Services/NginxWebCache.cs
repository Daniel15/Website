using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Daniel15.Data.Entities.Blog;
using Daniel15.Web.Extensions;
using Elmah;

namespace Daniel15.Web.Services
{
	/// <summary>
	/// Manages the front-end Nginx for the site
	/// </summary>
	/// <remarks>
	/// See http://wiki.nginx.org/HttpFastcgiModule#fastcgi_cache for cache configuration instructions
	/// This class requires the cache purge module (http://labs.frickle.com/nginx_ngx_cache_purge/) 
	/// to be installed.
	/// </remarks>
	public class NginxWebCache : IWebCache
	{
		private readonly UrlHelper _urlHelper;

		// TODO: Use dependency injection for this.
		public NginxWebCache()
		{
			// TODO: Ugly hack until I out how to do this properly - http://simpleinjector.codeplex.com/discussions/430939
			try
			{
				_urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
			}
			catch (HttpException)
			{
				// Thrown if we're not in a request (like when validating IoC config)
				// Just ignore it for now.
			}
				
		}

		/*public NginxWebCache(UrlHelper urlHelper)
		{
			_urlHelper = urlHelper;
		}*/

		/// <summary>
		/// Clear the cache for this blog post
		/// </summary>
		/// <param name="post">The blog post</param>
		public void ClearCache(PostSummaryModel post)
		{
			ClearCache(_urlHelper.BlogPostAbsolute(post));
			ClearCache(_urlHelper.ActionAbsolute(MVC.Site.Index()));
			ClearCache(_urlHelper.ActionAbsolute(MVC.Blog.Index()));
			// TODO: Clear tags and categories if they're ever cached
		}

		private void ClearCache(string url)
		{
			var client = new WebClient();
			try
			{
				var response = client.UploadString(url, "PURGE", string.Empty);

				// Check for expected response from Nginx
				if (!response.Contains("Successful purged"))
				{
					// This is not fatal, but log it in the error log anyways.
					ErrorSignal.FromCurrentContext().Raise(new Exception(string.Format("Unexpected response while purging cache for {0}: {1}", url, response)));
				}
			}
			catch (WebException ex)
			{
				// Check if the response was a 404
				if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
				{
					var response = (HttpWebResponse) ex.Response;
					if (response.StatusCode == HttpStatusCode.NotFound)
					{
						// Ignore 404 errors as they just mean that the page wasn't cached
						return;
					}
				}

				// A WebException that's not a 404... This isn't fatal but log it in the error log anyways
				ErrorSignal.FromCurrentContext().Raise(new Exception(string.Format("WebException while purging cache for {0}", url), ex));
			}
		}
	}
}