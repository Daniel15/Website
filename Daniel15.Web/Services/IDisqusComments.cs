namespace Daniel15.Web.Services
{
	/// <summary>
	/// Handles synchronisation of comments between Disqus and the local database.
	/// </summary>
	public interface IDisqusComments
	{
		/// <summary>
		/// Synchronise all comments on Disqus into the local database
		/// </summary>
		void Sync();
	}
}