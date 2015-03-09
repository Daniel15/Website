using Daniel15.Data.Entities.Blog;

namespace Daniel15.Web.Services
{
	/// <summary>
	/// A dummy implementation of <see cref="IWebCache"/> that does nothing.
	/// </summary>
	public class NullWebCache : IWebCache
	{
		/// <summary>
		/// Clear the cache of the home page
		/// </summary>
		public void ClearHomeCache()
		{
		}

		/// <summary>
		/// Clear the cache for this blog post
		/// </summary>
		/// <param name="post">The blog post</param>
		public void ClearCache(PostModel post)
		{
		}
	}
}