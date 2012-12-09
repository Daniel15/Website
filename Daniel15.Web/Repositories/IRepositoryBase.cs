using System.Collections.Generic;

namespace Daniel15.Web.Repositories
{
	/// <summary>
	/// Repository for accessing the database
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IRepositoryBase<T>
	{
		/// <summary>
		/// Gets all the entities in the database
		/// </summary>
		/// <returns></returns>
		List<T> All();

		/// <summary>
		/// Gets a particular entity from the database
		/// </summary>
		/// <param name="id">ID of the entity</param>
		/// <returns>The entity</returns>
		T Get(int id);

		/// <summary>
		/// Saves this entity to the database
		/// </summary>
		/// <param name="entity">The entity to save</param>
		void Save(T entity);

		/// <summary>
		/// Get the total number of records in this table
		/// </summary>
		/// <returns>Total number of records</returns>
		int Count();
	}
}
