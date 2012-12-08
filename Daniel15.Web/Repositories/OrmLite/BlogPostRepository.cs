using System.Collections.Generic;
using System.Data;
using Daniel15.Web.Models;
using ServiceStack.OrmLite;

namespace Daniel15.Web.Repositories.OrmLite
{
	/// <summary>
	/// Blog repository that uses OrmLite as the data access component
	/// </summary>
	public class BlogPostRepository : RepositoryBase<BlogPostModel>, IBlogPostRepository
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BlogPostRepository" /> class.
		/// </summary>
		/// <param name="conn">The database connection</param>
		public BlogPostRepository(IDbConnection conn) : base(conn)
		{
		}

		/// <summary>
		/// Gets the latest blog posts
		/// </summary>
		/// <returns>Latest blog posts</returns>
		public List<BlogPostModel> LatestPosts()
		{
			return _conn.Select<BlogPostModel>(query => query.OrderByDescending(post => post.Date));
		}
	}
}