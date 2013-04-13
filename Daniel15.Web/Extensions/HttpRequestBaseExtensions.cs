using System.Web;

namespace Daniel15.Web.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="HttpRequestBase"/>.
	/// </summary>
	public static class HttpRequestBaseExtensions
	{
		/// <summary>
		/// Determines if the specified request should redirect to FeedBurner. This is the case when
		/// a user accesses an RSS feed directly. Allows explicit access to the feed by adding a
		/// feedburner_override GET parameter. 
		/// </summary>
		/// <param name="request">The request</param>
		/// <returns><c>true</c> if this request can redirect to FeedBurner</returns>
		public static bool ShouldRedirectToFeedburner(this HttpRequestBase request)
		{
			var userAgent = (request.UserAgent ?? string.Empty).ToLower();
			return
				!userAgent.Contains("feedburner")
				&& !userAgent.Contains("feedvalidator")
				&& request.QueryString["feedburner_override"] == null;
		}
	}
}