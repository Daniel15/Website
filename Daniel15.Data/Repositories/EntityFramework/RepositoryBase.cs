using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Daniel15.Data.Repositories.EntityFramework
{
	/// <summary>
	/// A base repository class that uses Entity Framework as the data access component.
	/// </summary>
	/// <typeparam name="T">Type contained in this repository</typeparam>
	public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class, new ()
	{
		/// <summary>
		/// Entity Framework database context.
		/// </summary>
		protected DatabaseContext Context { get; }

		/// <summary>
		/// Gets the <see cref="DbSet{TEntity}"/> represented by this repository.
		/// </summary>
		protected abstract DbSet<T> Set { get; }

		/// <summary>
		/// Creates a new <see cref="RepositoryBase{T}"/>.
		/// </summary>
		/// <param name="context"></param>
		protected RepositoryBase(DatabaseContext context)
		{
			Context = context;
		}

		/// <summary>
		/// Gets all the entities in the database
		/// </summary>
		/// <returns></returns>
		public virtual List<T> All()
		{
			return Set.ToList();
		}

		/// <summary>
		/// Gets a particular entity from the database
		/// </summary>
		/// <param name="id">ID of the entity</param>
		/// <returns>The entity</returns>
		public virtual T Get(int id)
		{
			var entity = Set.Find(id);
			if (entity == null)
			{
				throw new EntityNotFoundException();
			}
			return entity;
		}

		/// <summary>
		/// Saves this entity to the database
		/// </summary>
		/// <param name="entity">The entity to save</param>
		public virtual void Save(T entity)
		{
			var isNew = Context.Entry(entity).State == EntityState.Detached;
			Save(entity, isNew);
		}

		/// <summary>
		/// Saves the entity to the database
		/// </summary>
		/// <param name="entity">The entity to save</param>
		/// <param name="isNew"><c>true</c> to do an INSERT or <c>false</c> to do an UPDATE</param>
		public virtual void Save(T entity, bool isNew)
		{
			if (isNew)
			{
				Set.Add(entity);
			}
			Context.SaveChanges();
		}

		/// <summary>
		/// Get the total number of records in this table
		/// </summary>
		/// <returns>Total number of records</returns>
		public virtual int Count()
		{
			return Set.Count();
		}
	}
}
