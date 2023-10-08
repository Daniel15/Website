using System.Linq.Expressions;
using Daniel15.Web.Exceptions;

namespace Daniel15.Web.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="IQueryable"/>.
	/// </summary>
	public static class QueryableExtensions
	{
		/// <summary>
		/// Returns the first item matching the predicate, or throws an exception
		/// </summary>
		/// <typeparam name="T">Entity type</typeparam>
		/// <param name="queryable">Data source</param>
		/// <param name="predicate">Predicate to match on</param>
		/// <returns>First item that matches the predicate</returns>
		/// <exception cref="EntityNotFoundException">Thrown if no item matched the predicate</exception>
		public static T FirstOrThrow<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate)
		{
			var entity = queryable.FirstOrDefault(predicate);
			if (entity == null)
			{
				throw new EntityNotFoundException();
			}
			return entity;
		}
	}
}
