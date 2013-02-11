using Daniel15.Data.Entities.Blog;

namespace Daniel15.Data.Repositories
{
	/// <summary>
	/// Repository for accessing Disqus comments
	/// </summary>
	public interface IDisqusCommentRepository : IRepositoryBase<DisqusCommentModel>
	{
		/// <summary>
		/// Load the specified comment, returning <c>null</c> if it's not in the database.
		/// </summary>
		/// <param name="id">Comment ID</param>
		/// <returns>Comment, or <c>null</c> if it doesn't exist</returns>
		DisqusCommentModel GetOrDefault(string id);
	}
}
