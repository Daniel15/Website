using System.Collections.Generic;
using Daniel15.Web.Models;
using Daniel15.Web.Models.Blog;

namespace Daniel15.Web.Repositories
{
	/// <summary>
	/// Repository for accessing blog posts
	/// </summary>
	public interface IBlogRepository : IRepositoryBase<PostModel>
	{
		/// <summary>
		/// Gets a post by slug.
		/// </summary>
		/// <param name="slug">The slug.</param>
		/// <returns>The post</returns>
		PostModel GetBySlug(string slug);

		/// <summary>
		/// Gets the categories for the specified blog post
		/// </summary>
		/// <param name="post">Blog post</param>
		/// <returns>Categories for this blog post</returns>
		IList<CategoryModel> CategoriesForPost(PostSummaryModel post);
			
		/// <summary>
		/// Gets the latest blog posts
		/// </summary>
		/// <returns>Latest blog posts</returns>
		List<PostModel> LatestPosts(int posts = 10);

		/// <summary>
		/// Gets a reduced DTO of the latest posts (essentially everything except content)
		/// </summary>
		/// <param name="posts">Number of posts to return</param>
		/// <returns>Blog post summary</returns>
		List<PostSummaryModel> LatestPostsSummary(int posts = 10);

		/// <summary>
		/// Gets the count of blog posts for every year and every month.
		/// </summary>
		/// <returns>A dictionary of years, which contains a dictionary of months and counts</returns>
		IDictionary<int, IDictionary<int, int>> MonthCounts();
	}
}
