using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Daniel15.BusinessLayer.Services
{
	/// <summary>
	/// Handles logging hits to short URLs
	/// </summary>
	public interface IShortUrlLogger
    {
		/// <summary>
		/// Logs a hit to the specified URL
		/// </summary>
		/// <param name="urlId">Shortened URL that was hit</param>
		/// <param name="httpContext">HTTP context of the request</param>
		Task LogHitAsync(int urlId, HttpContext httpContext, CancellationToken token = default(CancellationToken));

		/// <summary>
		/// Logs a hit to the specified URL
		/// </summary>
		/// <param name="urlId">Shortened URL that was hit</param>
		/// <param name="ip">IP the hit came from</param>
		/// <param name="userAgent">User-Agent the hit came from</param>
		/// <param name="referrer">HTTP Referrer the hit came from</param>
		Task LogHitAsync(int urlId, IPAddress ip, string userAgent, string referrer, CancellationToken token = default(CancellationToken));

    }
}
