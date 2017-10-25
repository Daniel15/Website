using System.Linq;
using System.Threading.Tasks;
using Daniel15.Data.Zurl.Entities;
using Daniel15.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Daniel15.Data.Zurl
{
	/// <summary>
	/// Handles loading data relating to shortened URLs.
	/// </summary>
	public class UrlRepository : IUrlRepository
	{
		private readonly ZurlDatabaseContext _context;

		public UrlRepository(ZurlDatabaseContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Loads a short URL by alias
		/// </summary>
		/// <param name="domain">Domain name of the short URL</param>
		/// <param name="alias">URL alias (that is, the string directly in the URL path)</param>
		/// <param name="maybeId">ID of the URL (base62 integer represented by the alias, or null if it is a custom alias)</param>
		/// <returns>The shortened URL, or <c>null</c> if no matching URL was found</returns>
		public ShortenedUrl TryGetByAlias(string domain, string alias, int? maybeId)
		{
			return _context.Urls
				.Where(x => x.Domain.Domain == domain)
				.OrderBy(x => x.Id)
				.FirstOrDefault(x => 
					(x.Type == "domain_custom" && x.CustomAlias == alias) ||
					(maybeId != null && x.Type == "domain" && x.DomainUrlId == maybeId)
				);
		}

		/// <summary>
		/// Saves data about a hit to a short URL
		/// </summary>
		/// <param name="hit">Hit to save</param>
		public async Task AddHitAsync(ShortenedUrlHit hit)
		{
			await _context.Hits.AddAsync(hit);
			await _context.Database.ExecuteSqlCommandAsync(
				"UPDATE urls SET last_hit = {0}, hits = hits + 1 WHERE id = {1}", 
				hit.Date.ToUnix(),
				hit.UrlId
			);

			await _context.SaveChangesAsync();
		}
	}
}
