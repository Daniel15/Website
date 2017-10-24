using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Daniel15.Data.Zurl.Entities
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

		[NotMapped]
	    public Uri Referrer
	    {
		    get
		    {
			    Uri.TryCreate(RawReferrer, UriKind.Absolute, out var uri);
			    return uri;
		    }
		    set => RawReferrer = value.ToString();
	    }
		public string ReferrerDomain { get; set; }

		/// <summary>
		/// Entity Framework doesn't support URI fields, so this is the  backing field
		/// for <see cref="Referrer"/>.
		/// </summary>
		public string RawReferrer { get; set; }

	    public DateTime Date { get; set; }
	}
}
