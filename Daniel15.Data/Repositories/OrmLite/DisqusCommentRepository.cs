using Daniel15.Data.Entities.Blog;
using ServiceStack.OrmLite;

namespace Daniel15.Data.Repositories.OrmLite
{
	/// <summary>
	/// Repository for accessing Disqus comments
	/// </summary>
	public class DisqusCommentRepository : RepositoryBase<DisqusCommentModel>, IDisqusCommentRepository
	{
		public DisqusCommentRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
		{
		}

		/// <summary>
		/// Load the specified comment, returning <c>null</c> if it's not in the database.
		/// </summary>
		/// <param name="id">Comment ID</param>
		/// <returns>Comment, or <c>null</c> if it doesn't exist</returns>
		public DisqusCommentModel GetOrDefault(string id)
		{
			return Connection.GetByIdOrDefault<DisqusCommentModel>(id);
		}
	}
}
