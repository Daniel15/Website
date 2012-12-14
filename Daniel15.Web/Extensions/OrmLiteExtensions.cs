using System;
using ServiceStack.DataAnnotations;

namespace Daniel15.Web.Repositories.OrmLite
{
	public static class OrmLiteExtensions
	{
		/// <summary>
		/// Gets the table name for this OrmLite entity
		/// </summary>
		/// <typeparam name="T">The entity type</typeparam>
		public static string GetTableName(this Type type)
		{
			// Get the type name as it might be the table name
			// TODO: See if OrmLite has a built-in function to get table name from entity
			var tableName = type.Name;

			// If entity has an AliasAttribute, use it for the name instead
			var attributes = type.GetCustomAttributes(typeof(AliasAttribute), true);
			if (attributes.Length > 0)
			{
				tableName = ((AliasAttribute)attributes[0]).Name;
			}

			return tableName;
		}
	}
}