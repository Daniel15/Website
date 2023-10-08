using System.Threading.Tasks;

namespace Daniel15.Web.Services
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
		/// <param name="ip">IP the hit came from</param>
		/// <param name="userAgent">User-Agent the hit came from</param>
		/// <param name="referrer">HTTP Referrer the hit came from</param>
		Task LogHitAsync(int urlId, string ip, string userAgent, string referrer);
    }
}
