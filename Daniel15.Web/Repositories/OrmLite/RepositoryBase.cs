using System.Collections.Generic;
using System.Data;
using ServiceStack.OrmLite;

namespace Daniel15.Web.Repositories.OrmLite
{
	/// <summary>
	/// A base repository class that uses OrmLite as the data access component.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : new()
	{
		protected readonly IDbConnection _conn;

		/// <summary>
		/// Initializes a new instance of the <see cref="RepositoryBase{T}" /> class.
		/// </summary>
		/// <param name="conn">The conn.</param>
		public RepositoryBase(IDbConnection conn)
		{
			_conn = conn;
		}

		/// <summary>
		/// Gets all the entities in the database
		/// </summary>
		/// <returns></returns>
		public List<T> All()
		{
			return _conn.Select<T>();
		}

		/// <summary>
		/// Gets a particular entity from the database
		/// </summary>
		/// <param name="id">ID of the entity</param>
		/// <returns>The entity</returns>
		public T Get(int id)
		{
			return _conn.GetById<T>(id);
		}

		/// <summary>
		/// Saves this entity to the database
		/// </summary>
		/// <param name="entity">The entity to save</param>
		public void Save(T entity)
		{
			_conn.Save(entity);
		}
	}
}