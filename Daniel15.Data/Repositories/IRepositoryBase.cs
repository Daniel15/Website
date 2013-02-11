using System.Collections.Generic;

namespace Daniel15.Data.Repositories
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
		/// Saves this entity to the database. First tries to load the entity to check if it exists
		/// If it exists, does an update. Otherwise, does an insert. If you know which operation you
		/// want to do, use the overload with the "isNew" parameter.
		/// </summary>
		/// <param name="entity">The entity to save</param>
		void Save(T entity);

		/// <summary>
		/// Saves the entity to the database
		/// </summary>
		/// <param name="entity">The entity to save</param>
		/// <param name="isNew"><c>true</c> to do an INSERT or <c>false</c> to do an UPDATE</param>
		void Save(T entity, bool isNew);

		/// <summary>
		/// Get the total number of records in this table
		/// </summary>
		/// <returns>Total number of records</returns>
		int Count();
	}
}
