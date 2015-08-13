using Microsoft.AspNet.Http;

namespace Daniel15.Web.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="HttpRequest"/>.
	/// </summary>
	public static class HttpRequestExtensions
	{
		/// <summary>
		/// Determines if the specified request should redirect to FeedBurner. This is the case when
		/// a user accesses an RSS feed directly. Allows explicit access to the feed by adding a
		/// feedburner_override GET parameter. 
		/// </summary>
		/// <param name="request">The request</param>
		/// <returns><c>true</c> if this request can redirect to FeedBurner</returns>
		public static bool ShouldRedirectToFeedburner(this HttpRequest request)
		{
			var userAgent = (request.Headers.Get("User-Agent") ?? string.Empty).ToLower();
			return
				!userAgent.Contains("feedburner")
				&& !userAgent.Contains("feedvalidator")
				&& !request.Query.ContainsKey("feedburner_override");
		}
	}
}
