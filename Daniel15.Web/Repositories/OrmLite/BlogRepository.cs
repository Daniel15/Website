using System;
using System.Collections.Generic;
using Daniel15.Web.Models;
using Daniel15.Web.Models.Blog;
using ServiceStack.OrmLite;

namespace Daniel15.Web.Repositories.OrmLite
{
	/// <summary>
	/// Blog repository that uses OrmLite as the data access component
	/// </summary>
	public class BlogRepository : RepositoryBase<PostModel>, IBlogRepository
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BlogRepository" /> class.
		/// </summary>
		/// <param name="connectionFactory">The database connection factory</param>
		public BlogRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
		{
		}

		/// <summary>
		/// Gets a post by slug.
		/// </summary>
		/// <param name="slug">The slug.</param>
		/// <returns>The post</returns>
		public PostModel GetBySlug(string slug)
		{
			try
			{
				return Connection.First<PostModel>(post => post.Slug == slug);
			}
			catch (ArgumentNullException)
			{
				// Post wasn't found
				throw new ItemNotFoundException();
			}
		}

		/// <summary>
		/// Gets the categories for the specified blog post
		/// </summary>
		/// <param name="post">Blog post</param>
		/// <returns>Categories for this blog post</returns>
		public IList<CategoryModel> CategoriesForPost(PostSummaryModel post)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the latest blog posts
		/// </summary>
		/// <returns>Latest blog posts</returns>
		public List<PostModel> LatestPosts(int posts = 10)
		{
			return Connection.Select<PostModel>(query => query
				.OrderByDescending(post => post.Date)
				.Limit(posts)
			);
		}

		/// <summary>
		/// Gets a reduced DTO of the latest posts (essentially everything except content)
		/// </summary>
		/// <param name="posts">Number of posts to return</param>
		/// <returns>Blog post summary</returns>
		public List<PostSummaryModel> LatestPostsSummary(int posts = 10)
		{
			return Connection.Select<PostSummaryModel>(query => query
				.OrderByDescending(post => post.Date)
				.Limit(posts)
			);
		}

		/// <summary>
		/// Gets the count of blog posts for every year and every month.
		/// </summary>
		/// <returns>A dictionary of years, which contains a dictionary of months and counts</returns>
		public IDictionary<int, IDictionary<int, int>> MonthCounts()
		{
			var counts = Connection.Select<MonthYearCount>(@"
SELECT MONTH(FROM_UNIXTIME(date)) AS month, YEAR(FROM_UNIXTIME(date)) AS year, COUNT(*) AS count
FROM blog_posts
GROUP BY year, month
ORDER BY year DESC, month DESC");

			IDictionary<int, IDictionary<int, int>> results = new Dictionary<int, IDictionary<int, int>>();

			foreach (var count in counts)
			{
				if (!results.ContainsKey(count.Year))
					results[count.Year] = new Dictionary<int, int>();

				results[count.Year][count.Month] = count.Count;
			}

			return results;
		}

		private class MonthYearCount
		{
			public int Month { get; set; }
			public int Year { get; set; }
			public int Count { get; set; }
		}
	}
}