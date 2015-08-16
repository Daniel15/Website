using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Daniel15.Data.Entities;
using Daniel15.Data.Entities.Blog;

namespace Daniel15.Data.Repositories.EntityFramework
{
	/// <summary>
	/// Repository for accessing Disqus comments
	/// </summary>
	public class DisqusCommentRepository : RepositoryBase<DisqusCommentModel>, IDisqusCommentRepository
	{
		/// <summary>
		/// Creates a new <see cref="DisqusCommentRepository"/>
		/// </summary>
		/// <param name="context">Entity Framework database context</param>
		public DisqusCommentRepository(DatabaseContext context) : base(context)
		{
		}

		/// <summary>
		/// Gets the <see cref="DbSet{TEntity}"/> represented by this repository.
		/// </summary>
		protected override DbSet<DisqusCommentModel> Set => Context.DisqusComments;

		/// <summary>
		/// Load the specified comment, returning <c>null</c> if it's not in the database.
		/// </summary>
		/// <param name="id">Comment ID</param>
		/// <returns>Comment, or <c>null</c> if it doesn't exist</returns>
		public DisqusCommentModel GetOrDefault(string id)
		{
			return Context.DisqusComments.Find(id);
		}

		/// <summary>
		/// Gets all the cached Disqus comments for this entity
		/// </summary>
		/// <param name="entity">Entity to get comments for</param>
		/// <returns>List of all the comments</returns>
		public IEnumerable<DisqusCommentModel> GetComments(ISupportsDisqus entity)
		{
			return Context.DisqusComments
				.Where(comm => comm.ThreadIdentifier == entity.DisqusIdentifier)
				.OrderBy(comm => comm.Date);
		}

		/// <summary>
		/// Gets all the cached Disqus posts for this entity, as a tree.
		/// </summary>
		/// <param name="entity">Entity to get comments for</param>
		/// <returns>List of all the root-level comments, with the Children properties populated</returns>
		public IEnumerable<DisqusCommentModel> GetCommentsTree(ISupportsDisqus entity)
		{
			var allComments = GetComments(entity).ToDictionary(comm => comm.Id);
			var rootComments = new List<DisqusCommentModel>();

			foreach (var comment in allComments)
			{
				// All comments start with no children
				comment.Value.Children = new List<DisqusCommentModel>();

				// Does it have a parent?
				if (!string.IsNullOrEmpty(comment.Value.ParentCommentId))
				{
					allComments[comment.Value.ParentCommentId].Children.Add(comment.Value);
				}
				else
				{
					rootComments.Add(comment.Value);
				}
			}

			return rootComments;
		}
	}
}
