using System.Configuration;
using System.Data;
using System.Data.Entity;
using MySql.Data.Entity;
using MySql.Data.MySqlClient;

namespace Daniel15.Data
{
	/// <summary>
	/// Configures the database (since we're using an old version of Entity Framework
	/// that requires configuration in Web.config, which ASP.NET 5 doesn't support)
	/// </summary>
	public class DatabaseConfiguration : DbConfiguration
	{
		public DatabaseConfiguration()
		{
			// Attempt to add MySQL ADO.NET providefr
			try
			{
				var dataSet = (DataSet) ConfigurationManager.GetSection("system.data");
				dataSet.Tables[0].Rows.Add(
					"MySQL Data Provider",
					".Net Framework Data Provider for MySQL",
					"MySql.Data.MySqlClient",
					typeof (MySqlClientFactory).AssemblyQualifiedName
					);
			}
			catch (ConstraintException)
			{
				// MySQL provider is already installed, just ignore the exception
			}

			// Configure Entity Framework
			SetProviderServices("MySql.Data.MySqlClient", new MySqlProviderServices());
			SetDefaultConnectionFactory(new MySqlConnectionFactory());
		}
	}
}
