using System.Text.RegularExpressions;
using Daniel15.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Daniel15.Web.Extensions
{
	/// <summary>
	/// Utility methods for database configuration.
	/// </summary>
	public static class DatabaseContextExtensions
	{
		/// <summary>
		/// Prefix for boolean fields in the database
		/// </summary>
		private const string BOOLEAN_PREFIX = "Is_";

		/// <summary>
		/// Regular expression matching segments in a camelcase string
		/// </summary>
		private static readonly Regex _camelCaseRegex = new Regex(".[A-Z]");

		/// <summary>
		/// Configures standard conventions for names of the database tables and fields
		/// </summary>
		/// <param name="modelBuilder">EF model builder</param>
		public static void ConfigureConventions(this ModelBuilder modelBuilder)
		{
			foreach (var entity in modelBuilder.Model.GetEntityTypes())
			{
				// Lowercase table names
				entity.SetTableName(entity.GetTableName().ToLowerInvariant());

				foreach (var property in entity.GetProperties())
				{
					// Use underscores for column names (eg. "AuthorProfileUrl" -> "author_profile_url"
					var name = _camelCaseRegex.Replace(
						property.Name,
						match => match.Value[0] + "_" + match.Value[1]
					);
					// Remove "is" prefix (eg. "IsPrimary" -> "Primary") and "Raw" prefix (eg. "RawTechnologies" => "Technologies")
					name = name.TrimStart(BOOLEAN_PREFIX).ToLowerInvariant();
					property.SetColumnName(name);
				}
			}
		}
	}
}
