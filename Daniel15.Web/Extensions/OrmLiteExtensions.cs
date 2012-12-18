using System;
using System.Data;
using System.Linq.Expressions;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

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

		/// <summary>
		/// Returns the first item matching the predicate, or throws an exception
		/// </summary>
		/// <typeparam name="T">Entity type</typeparam>
		/// <param name="connection">Database connection</param>
		/// <param name="predicate">Predicate to match on</param>
		/// <returns>First item that matches the predicate</returns>
		/// <exception cref="ItemNotFoundException">Thrown if no item matched the predicate</exception>
		public static T FirstOrThrow<T>(this IDbConnection connection, Expression<Func<T, bool>> predicate) where T : new()
		{
			var entity = connection.FirstOrDefault(predicate);
			if (entity == null)
				throw new ItemNotFoundException();

			return entity;
		}
	}
}