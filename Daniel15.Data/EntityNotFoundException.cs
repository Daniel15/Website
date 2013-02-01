using System;

namespace Daniel15.Data
{
	/// <summary>
	/// Thrown when an item is not found in the database
	/// </summary>
	public class EntityNotFoundException : ApplicationException
	{
		public EntityNotFoundException()
			: base("The specified entity was not found in the database")
		{	
		}
	}
}