using Daniel15.Data.Extensions;
using Daniel15.Data.Zurl.Entities;
using Microsoft.EntityFrameworkCore;

namespace Daniel15.Data.Zurl
{
	/// <summary>
	/// Database context for the zurl.ws URL shortening database.
	/// </summary>
	public class ZurlDatabaseContext : DbContext
	{
		/// <summary>
		/// Creates a new instance of <see cref="ZurlDatabaseContext"/>.
		/// </summary>
		public ZurlDatabaseContext(DbContextOptions<ZurlDatabaseContext> options) 
			: base(options) {}

		public virtual DbSet<ShortenedUrl> Urls { get; set; }

		/// <summary>
		/// Initialises the Entity Framework model
		/// </summary>
		/// <param name="modelBuilder">EF model builder</param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ConfigureConventions();
		}
	}
}
