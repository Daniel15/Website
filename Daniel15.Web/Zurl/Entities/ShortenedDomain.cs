using System.ComponentModel.DataAnnotations.Schema;

namespace Daniel15.Web.Zurl.Entities
{
	/// <summary>
	/// Represents a custom domain for the zurl.ws URL shortener.
	/// </summary>
	[Table("domains")]
    public class ShortenedDomain
    {
		public int Id { get; set; }
		public string Domain { get; set; }
    }
}
