using System.Threading.Tasks;
using Daniel15.Data.Zurl.Entities;

namespace Daniel15.Data.Zurl
{
	/// <summary>
	/// Handles loading data relating to shortened URLs.
	/// </summary>
	public interface IUrlRepository
	{
		/// <summary>
		/// Loads a short URL by alias
		/// </summary>
		/// <param name="domain">Domain name of the short URL</param>
		/// <param name="alias">URL alias (that is, the string directly in the URL path)</param>
		/// <param name="maybeId">ID of the URL (base62 integer represented by the alias, or null if it is a custom alias)</param>
		/// <returns>The shortened URL, or <c>null</c> if no matching URL was found</returns>
		ShortenedUrl TryGetByAlias(string domain, string alias, int? maybeId);

		/// <summary>
		/// Saves data about a hit to a short URL
		/// </summary>
		/// <param name="hit">Hit to save</param>
		Task AddHitAsync(ShortenedUrlHit hit);
	}
}
