using System.Collections.Generic;
using System.Data;
using Daniel15.Web.Models;
using ServiceStack.OrmLite;

namespace Daniel15.Web.Repositories.OrmLite
{
	/// <summary>
	/// Blog repository that uses OrmLite as the data access component
	/// </summary>
	public class BlogRepository : RepositoryBase<BlogPostModel>, IBlogPostRepository
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BlogRepository" /> class.
		/// </summary>
		/// <param name="connectionFactory">The database connection factory</param>
		public BlogRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
		{
		}

		/// <summary>
		/// Gets the latest blog posts
		/// </summary>
		/// <returns>Latest blog posts</returns>
		public List<BlogPostModel> LatestPosts(int posts = 10)
		{
			return Connection.Select<BlogPostModel>(query => query
				.OrderByDescending(post => post.Date)
				.Limit(posts)
			);
		}

		/// <summary>
		/// Gets a reduced DTO of the latest posts (essentially everything except content)
		/// </summary>
		/// <param name="posts">Number of posts to return</param>
		/// <returns>Blog post summary</returns>
		public List<BlogPostSummaryModel> LatestPostsSummary(int posts = 10)
		{
			return Connection.Select<BlogPostSummaryModel>(query => query
				.OrderByDescending(post => post.Date)
				.Limit(posts)
			);
		}
	}
}