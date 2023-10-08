using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Daniel15.Web.Zurl.Entities
{
	/// <summary>
	/// Represents a hit to a URL shortened with the zurl.ws URL shortener.
	/// </summary>
	[Table("hits")]
	public class ShortenedUrlHit
    {
		public int Id { get; set; }
		public int UrlId { get; set; }
		public ShortenedUrl Url { get; set; }
		public string UserAgent { get; set; }
		public string Browser { get; set; }
		public string BrowserVersion { get; set; }
		public string IpAddress { get; set; }
		public string Country { get; set; }

	    public Uri Referrer { get; set; }
		public string ReferrerDomain { get; set; }

		public DateTime Date { get; set; }
	}
}
