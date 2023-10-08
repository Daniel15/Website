using System.Collections.Generic;
using Daniel15.Web.Models;
using Daniel15.Web.Models.Blog;

namespace Daniel15.Web.Repositories
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

		/// <summary>
		/// Gets all the cached Disqus comments for this entity
		/// </summary>
		/// <param name="entity">Entity to get comments for</param>
		/// <returns>List of all the comments</returns>
		IEnumerable<DisqusCommentModel> GetComments(ISupportsDisqus entity);

		/// <summary>
		/// Gets all the cached Disqus posts for this entity, as a tree.
		/// </summary>
		/// <param name="entity">Entity to get comments for</param>
		/// <returns>List of all the root-level comments, with the Children properties populated</returns>
		IEnumerable<DisqusCommentModel> GetCommentsTree(ISupportsDisqus entity);
	}
}
