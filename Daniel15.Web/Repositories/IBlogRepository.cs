using System.Collections.Generic;
using Daniel15.Web.Models;

namespace Daniel15.Web.Repositories
{
	/// <summary>
	/// Repository for accessing blog posts
	/// </summary>
	public interface IBlogRepository : IRepositoryBase<BlogPostModel>
	{
		/// <summary>
		/// Gets the latest blog posts
		/// </summary>
		/// <returns>Latest blog posts</returns>
		List<BlogPostModel> LatestPosts(int posts = 10);

		/// <summary>
		/// Gets a reduced DTO of the latest posts (essentially everything except content)
		/// </summary>
		/// <param name="posts">Number of posts to return</param>
		/// <returns>Blog post summary</returns>
		List<BlogPostSummaryModel> LatestPostsSummary(int posts = 10);

		/// <summary>
		/// Gets the count of blog posts for every year and every month.
		/// </summary>
		/// <returns>A dictionary of years, which contains a dictionary of months and counts</returns>
		IDictionary<int, IDictionary<int, int>> MonthCounts();
	}
}
