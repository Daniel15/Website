using System.Collections.Generic;
using Daniel15.Data.Entities.Blog;

namespace Daniel15.Data.Repositories
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
		/// Gets a post summary by slug.
		/// </summary>
		/// <param name="slug">The slug.</param>
		/// <returns>The post</returns>
		PostSummaryModel GetSummaryBySlug(string slug);

		/// <summary>
		/// Gets the categories for the specified blog post
		/// </summary>
		/// <param name="post">Blog post</param>
		/// <returns>Categories for this blog post</returns>
		IList<CategoryModel> CategoriesForPost(PostSummaryModel post);

		/// <summary>
		/// Gets the tags for the specified blog post
		/// </summary>
		/// <param name="post">Blog post</param>
		/// <returns>Tags for this blog post</returns>
		IList<TagModel> TagsForPost(PostSummaryModel post);
			
		/// <summary>
		/// Gets the latest blog posts
		/// </summary>
		/// <param name="count">Number of posts to return</param>
		/// <param name="offset">Post to start at</param>
		/// <returns>Latest blog posts</returns>
		List<PostModel> LatestPosts(int count = 10, int offset = 0);

		/// <summary>
		/// Gets the latest blog posts in this category
		/// </summary>
		/// <param name="category">Category to get posts from</param>
		/// <param name="count">Number of posts to return</param>
		/// <param name="offset">Post to start at</param>
		/// <returns>Latest blog posts</returns>
		List<PostModel> LatestPosts(CategoryModel category, int count = 10, int offset = 0);

		/// <summary>
		/// Gets the latest blog posts in this tag
		/// </summary>
		/// <param name="tag">Tag to get posts from</param>
		/// <param name="count">Number of posts to return</param>
		/// <param name="offset">Post to start at</param>
		/// <returns>Latest blog posts</returns>
		List<PostModel> LatestPosts(TagModel tag, int count = 10, int offset = 0);

		/// <summary>
		/// Gets the latest blog posts for the specified year and month
		/// </summary>
		/// /// <param name="year">Year to get posts for</param>
		/// <param name="month">Month to get posts for</param>
		/// <param name="count">Number of posts to return</param>
		/// <param name="offset">Post to start at</param>
		/// <returns>Latest blog posts</returns>
		List<PostModel> LatestPostsForMonth(int year, int month, int count = 10, int offset = 0);

		/// <summary>
		/// Gets a reduced DTO of the latest posts (essentially everything except content)
		/// </summary>
		/// <param name="count">Number of posts to return</param>
		/// <param name="published">Whether to return published posts (true) or unpublished (false)</param>
		/// <returns>Blog post summary</returns>
		List<PostSummaryModel> LatestPostsSummary(int count = 10, bool published = true);

		/// <summary>
		/// Gets the count of blog posts for every year and every month.
		/// </summary>
		/// <returns>A dictionary of years, which contains a dictionary of months and counts</returns>
		IDictionary<int, IDictionary<int, int>> MonthCounts();

		/// <summary>
		/// Gets a category by slug
		/// </summary>
		/// <param name="slug">Slug of the category</param>
		/// <returns>The category</returns>
		CategoryModel GetCategory(string slug);

		/// <summary>
		/// Gets a tag by slug
		/// </summary>
		/// <param name="slug">Slug of the tag</param>
		/// <returns>The tag</returns>
		TagModel GetTag(string slug);

		/// <summary>
		/// Get the total number of posts that are published
		/// </summary>
		/// <returns>Total number of posts</returns>
		int PublishedCount();

		/// <summary>
		/// Get the total number of posts that are published in this category
		/// </summary>
		/// <returns>Total number of posts in the category</returns>
		int PublishedCount(CategoryModel category);

		/// <summary>
		/// Get the total number of posts that are published in this tag
		/// </summary>
		/// <returns>Total number of posts in the tag</returns>
		int PublishedCount(TagModel tag);

		/// <summary>
		/// Get the total number of posts that are not yet published
		/// </summary>
		/// <returns>Total number of posts that have not yet been published</returns>
		int UnpublishedCount();

		/// <summary>
		/// Get the total number of posts that are published in this month and year
		/// </summary>
		/// <param name="year">Year to get count for</param>
		/// <param name="month">Month to get count for</param>
		/// <returns>Total number of posts that were posted in this month</returns>
		int PublishedCountForMonth(int year, int month);

		/// <summary>
		/// Get an alphabetical list of available categories
		/// </summary>
		/// <returns>A list of categories</returns>
		IList<CategoryModel> Categories();

		/// <summary>
		/// Get an alphabetical list of available tags
		/// </summary>
		/// <returns>A list of tags</returns>
		IList<TagModel> Tags();

		/// <summary>
		/// Set the categories this blog post is categorised under
		/// </summary>
		/// <param name="post">The post</param>
		/// <param name="categoryIds">Category IDs</param>
		void SetCategories(PostSummaryModel post, IEnumerable<int> categoryIds);

		/// <summary>
		/// Set the tags this blog post is tagged with
		/// </summary>
		/// <param name="post">The post</param>
		/// <param name="tagIds">Tag IDs</param>
		void SetTags(PostSummaryModel post, IEnumerable<int> tagIds);
	}
}
