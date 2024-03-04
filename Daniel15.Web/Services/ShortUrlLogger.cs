using System.Net;
using Coravel.Invocable;
using Daniel15.Web.Zurl;
using Daniel15.Web.Zurl.Entities;
using MaxMind.GeoIP2;
using UAParser;

namespace Daniel15.Web.Services
{
	/// <summary>
	/// Handles logging hits to short URLs
	/// </summary>
	public class ShortUrlLogger : IInvocable, IInvocableWithPayload<ShortUrlLogger.Input>
	{
		private readonly IUrlRepository _urlRepository;
		private readonly IGeoIP2DatabaseReader _geoIp;
		private readonly Parser _uaParser;

		public ShortUrlLogger(IUrlRepository urlRepository, IGeoIP2DatabaseReader geoIp, Parser uaParser)
		{
			_urlRepository = urlRepository;
			_geoIp = geoIp;
			_uaParser = uaParser;
		}

		public Input Payload { get; set; }

		/// <summary>
		/// Logs a hit to the specified URL
		/// </summary>
		public async Task Invoke()
		{
			var hit = CreateHit(
				Payload.UrlId,
				IPAddress.Parse(Payload.Ip),
				Payload.UserAgent,
				Payload.Referrer
			);
			await _urlRepository.AddHitAsync(hit);
		}

		/// <summary>
		/// Creates a <see cref="ShortenedUrlHit"/> representing a hit to the specified URL
		/// </summary>
		/// <param name="urlId">Shortened URL that was hit</param>
		/// <param name="ip">IP the hit came from</param>
		/// <param name="userAgent">User-Agent the hit came from</param>
		/// <param name="referrer">HTTP Referrer the hit came from</param>
		/// <returns>The hit. Not saved to the database yet.</returns>
		private ShortenedUrlHit CreateHit(int urlId, IPAddress ip, string userAgent, string referrer)
		{
			var hit = new ShortenedUrlHit
			{
				UrlId = urlId,
				Date = DateTime.Now,
				UserAgent = userAgent,
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
				var parsedUserAgent = _uaParser.ParseUserAgent(hit.UserAgent);
				hit.Browser = parsedUserAgent.Family;
				hit.BrowserVersion = parsedUserAgent.Major + "." + parsedUserAgent.Minor + "." + parsedUserAgent.Patch;
			}

			// Determine GeoIP country
			if (_geoIp.TryCountry(ip, out var geoIpResponse))
			{
				hit.Country = geoIpResponse.Country.IsoCode;
			}

			// Parse domain from referrer
			if (Uri.TryCreate(referrer, UriKind.Absolute, out var referrerUri))
			{
				hit.Referrer = referrerUri;
				hit.ReferrerDomain = referrerUri.Host.Replace("www.", string.Empty);
			}

			return hit;
		}

		/// <summary>
		/// Data to log a hit to a short URL
		/// </summary>
		/// <param name="UrlId">Shortened URL that was hit</param>
		/// <param name="Ip">IP the hit came from</param>
		/// <param name="UserAgent">User-Agent the hit came from</param>
		/// <param name="Referrer">HTTP Referrer the hit came from</param>
		public readonly record struct Input(
			int UrlId,
			string Ip,
			string UserAgent,
			string Referrer
		);
	}
}
