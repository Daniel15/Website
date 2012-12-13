using System;
using System.Collections.Generic;
using Daniel15.Web.Models;
using Daniel15.Web.Models.Blog;
using ServiceStack.OrmLite;
using System.Linq;

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
			var post = Connection.FirstOrDefault<PostModel>(x => x.Slug == slug);

			// Check if post wasn't found
			if (post == null)
				throw new ItemNotFoundException();

			// Get the main category as well
			// TODO: Do this using a join in the above query instead
			post.MainCategory = Connection.First<CategoryModel>(x => x.Id == post.MainCategoryId);

			return post;
		}

		/// <summary>
		/// Gets the categories for the specified blog post
		/// </summary>
		/// <param name="post">Blog post</param>
		/// <returns>Categories for this blog post</returns>
		public IList<CategoryModel> CategoriesForPost(PostSummaryModel post)
		{
			return Connection.Select<CategoryModel>(@"
				SELECT blog_categories.id, blog_categories.title, blog_categories.slug
				FROM blog_post_categories
				INNER JOIN blog_categories ON blog_categories.id = blog_post_categories.category_id
				WHERE blog_post_categories.post_id = {0}", post.Id);
		}

		/// <summary>
		/// Gets the latest blog posts
		/// </summary>
		/// <param name="count">Number of posts to return</param>
		/// <param name="offset">Post to start at</param>
		/// <returns>Latest blog posts</returns>
		public List<PostModel> LatestPosts(int count = 10, int offset = 0)
		{
			var posts = Connection.Select<PostModel>(query => query
				.OrderByDescending(post => post.Date)
				.Limit(offset, count)
			);

			// Get all the main categories as well
			// TODO: Use a join for this!! Figure out how best to do it with OrmLite.
			var categoryIds = posts.Select(post => post.MainCategoryId).Distinct();
			//var categories = Connection.Select<CategoryModel>(cat => Sql.In(cat.Id, categoryIds)).ToDictionary(x => x.Id); // Sql.In() expects param array, didn't work
			var categories = Connection.Select<CategoryModel>("id IN (" + string.Join(", ", categoryIds) + ")").ToDictionary(x => x.Id);

			foreach (var post in posts)
				post.MainCategory = categories[post.MainCategoryId];

			return posts;
		}

		/// <summary>
		/// Gets a reduced DTO of the latest posts (essentially everything except content)
		/// </summary>
		/// <param name="count">Number of posts to return</param>
		/// <returns>Blog post summary</returns>
		public List<PostSummaryModel> LatestPostsSummary(int count = 10)
		{
			return Connection.Select<PostSummaryModel>(query => query
				.OrderByDescending(post => post.Date)
				.Limit(count)
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