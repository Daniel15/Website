using System.ComponentModel.DataAnnotations.Schema;

namespace Daniel15.Web.Zurl.Entities
{
	/// <summary>
	/// Represents a URL shortened with the zurl.ws URL shortener.
	/// </summary>
	[Table("urls")]
    public class ShortenedUrl
    {
		public int Id { get; set; }
		public string Url { get; set; }
		public int Hits { get; set; }
		public string Type { get; set; }

	    public string CustomAlias { get; set; }

	    public int? DomainUrlId { get; set; }
		
	    public int DomainId { get; set; }
	    public ShortenedDomain Domain { get; set; }
	}
}
