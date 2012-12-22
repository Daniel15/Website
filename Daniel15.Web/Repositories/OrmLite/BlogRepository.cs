using System;
using System.Collections.Generic;
using Daniel15.Web.Models.Blog;
using ServiceStack.OrmLite;
using System.Linq;
using Daniel15.Web.Extensions;

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
			var post = Connection.FirstOrThrow<PostModel>(x => x.Slug == slug);
			// Get the main category as well
			// TODO: Do this using a join in the above query instead
			post.MainCategory = Connection.First<CategoryModel>(x => x.Id == post.MainCategoryId);

			return post;
		}

		/// <summary>
		/// Gets a post summary by slug.
		/// </summary>
		/// <param name="slug">The slug.</param>
		/// <returns>The post</returns>
		public PostSummaryModel GetSummaryBySlug(string slug)
		{
			return Connection.FirstOrThrow<PostSummaryModel>(x => x.Slug == slug);
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
		/// Gets the tags for the specified blog post
		/// </summary>
		/// <param name="post">Blog post</param>
		/// <returns>Tags for this blog post</returns>
		public IList<TagModel> TagsForPost(PostSummaryModel post)
		{
			return Connection.Select<TagModel>(@"
				SELECT blog_tags.id, blog_tags.title, blog_tags.slug
				FROM blog_post_tags
				INNER JOIN blog_tags ON blog_tags.id = blog_post_tags.tag_id
				WHERE blog_post_tags.post_id = {0}", post.Id);
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
				.Where(post => post.Published)
				.OrderByDescending(post => post.Date)
				.Limit(offset, count)
			);

			AddMainCategories(posts);
			return posts;
		}

		/// <summary>
		/// Gets the latest blog posts in this category
		/// </summary>
		/// <param name="category">Category to get posts from</param>
		/// <param name="count">Number of posts to return</param>
		/// <param name="offset">Post to start at</param>
		/// <returns>Latest blog posts</returns>
		public List<PostModel> LatestPosts(CategoryModel category, int count = 10, int offset = 0)
		{
			return LatestCategoryOrTag("category", "categories", category.Id, count, offset);
		}

		/// <summary>
		/// Gets the latest blog posts in this tag
		/// </summary>
		/// <param name="tag">Tag to get posts from</param>
		/// <param name="count">Number of posts to return</param>
		/// <param name="offset">Post to start at</param>
		/// <returns>Latest blog posts</returns>
		public List<PostModel> LatestPosts(TagModel tag, int count = 10, int offset = 0)
		{
			return LatestCategoryOrTag("tag", "tags", tag.Id, count, offset);
		}

		/// <summary>
		/// Gets the latest blog posts in this category or tag
		/// </summary>
		/// <param name="typeSingular">Singular type name ("tag" or "category")</param>
		/// <param name="typePlural">Plural type name ("tags" or "categories")</param>
		/// <param name="count">Number of posts to return</param>
		/// <param name="offset">Post to start at</param>
		/// <returns>Latest blog posts in the specified category or tag</returns>
		private List<PostModel> LatestCategoryOrTag(string typeSingular, string typePlural, int id, int count, int offset)
		{
			var posts = Connection.Select<PostModel>(@"
				SELECT id, title, slug, published, date, content, maincategory_id
				FROM blog_post_" + typePlural + @"
				INNER JOIN blog_posts ON blog_posts.id = blog_post_" + typePlural + @".post_id
				WHERE blog_post_" + typePlural + @"." + typeSingular + @"_id = {0}
					AND blog_posts.published = 1
				ORDER BY blog_posts.date DESC
				LIMIT {1}, {2}", id, offset, count, typePlural, typeSingular);

			AddMainCategories(posts);
			return posts;
		}

		/// <summary>
		/// Gets the latest blog posts for the specified year and month
		/// </summary>
		/// /// <param name="year">Year to get posts for</param>
		/// <param name="month">Month to get posts for</param>
		/// <param name="count">Number of posts to return</param>
		/// <param name="offset">Post to start at</param>
		/// <returns>Latest blog posts</returns>
		public List<PostModel> LatestPostsForMonth(int year, int month, int count = 10, int offset = 0)
		{
			var firstDate = new DateTime(year, month, day: 1);
			var lastDate = firstDate.AddMonths(1);

			var posts = Connection.Select<PostModel>(query => query
				.Where(post => 
					post.Published 
					&& post.UnixDate >= firstDate.ToUnix()
					&& post.UnixDate < lastDate.ToUnix()
				)
				.OrderByDescending(post => post.Date)
				.Limit(offset, count)
			);

			AddMainCategories(posts);
			return posts;
		}

		/// <summary>
		/// Loads all the main category information for the blog posts, and sets
		/// the MainCategory column on the posts
		/// TODO: Use a join for this!! Figure out how to do it with OrmLite
		/// </summary>
		/// <param name="posts"></param>
		private void AddMainCategories(IList<PostModel> posts)
		{
			var categoryIds = posts.Select(post => post.MainCategoryId).Distinct();
			if (!categoryIds.Any())
				return;

			//var categories = Connection.Select<CategoryModel>(cat => Sql.In(cat.Id, categoryIds)).ToDictionary(x => x.Id); // Sql.In() expects param array, didn't work
			var categories = Connection.Select<CategoryModel>("id IN (" + string.Join(", ", categoryIds) + ")").ToDictionary(x => x.Id);

			foreach (var post in posts)
				post.MainCategory = categories[post.MainCategoryId];
		}

		/// <summary>
		/// Gets a reduced DTO of the latest posts (essentially everything except content)
		/// </summary>
		/// <param name="count">Number of posts to return</param>
		/// <param name="published">Whether to return published posts (true) or unpublished (false)</param>
		/// <returns>Blog post summary</returns>
		public List<PostSummaryModel> LatestPostsSummary(int count = 10, bool published = true)
		{
			return Connection.Select<PostSummaryModel>(query => query
				.Where(post => post.Published == published)
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

		/// <summary>
		/// Gets a category by slug
		/// </summary>
		/// <param name="slug">Slug of the category</param>
		/// <returns>The category</returns>
		public CategoryModel GetCategory(string slug)
		{
			return Connection.FirstOrThrow<CategoryModel>(x => x.Slug == slug);
		}

		/// <summary>
		/// Gets a tag by slug
		/// </summary>
		/// <param name="slug">Slug of the tag</param>
		/// <returns>The tag</returns>
		public TagModel GetTag(string slug)
		{
			return Connection.FirstOrThrow<TagModel>(tag => tag.Slug == slug);
		}

		/// <summary>
		/// Get the total number of posts that are published
		/// </summary>
		/// <returns>Total number of posts</returns>
		public int PublishedCount()
		{
			return Connection.GetScalar<int>("SELECT COUNT(*) FROM blog_posts WHERE published = 1");
		}

		/// <summary>
		/// Get the total number of posts that are published in this category
		/// </summary>
		/// <returns>Total number of posts in the category</returns>
		public int PublishedCount(CategoryModel category)
		{
			return PublishedCountCategoryOrTag("category", "categories", category.Id);
		}

		/// <summary>
		/// Get the total number of posts that are published in this tag
		/// </summary>
		/// <returns>Total number of posts in the tag</returns>
		public int PublishedCount(TagModel tag)
		{
			return PublishedCountCategoryOrTag("tag", "tags", tag.Id);
		}

		/// <summary>
		/// Get the total number of posts that are not yet published
		/// </summary>
		/// <returns>Total number of posts that have not yet been published</returns>
		public int UnpublishedCount()
		{
			return Connection.GetScalar<int>("SELECT COUNT(*) FROM blog_posts WHERE published = 0");
		}

		/// <summary>
		/// Get the total number of posts that are published in this tag or category
		/// </summary>
		/// <param name="typeSingular">Singular type name ("tag" or "category")</param>
		/// <param name="typePlural">Plural type name ("tags" or "categories")</param>
		/// <param name="id">ID of the tag or category</param>
		/// <returns>Total number of posts in the tag/category</returns>
		private int PublishedCountCategoryOrTag(string typeSingular, string typePlural, int id)
		{
			return Connection.GetScalar<int>(@"
				SELECT COUNT(*) 
				FROM blog_post_" + typePlural + @" 
				INNER JOIN blog_posts ON blog_posts.id = blog_post_" + typePlural + @".post_id
				WHERE blog_post_" + typePlural + @"." + typeSingular + @"_id = {0}
					AND blog_posts.published = 1", id);
		}

		/// <summary>
		/// Get the total number of posts that are published in this month and year
		/// </summary>
		/// <param name="year">Year to get count for</param>
		/// <param name="month">Month to get count for</param>
		/// <returns>Total number of posts that were posted in this month</returns>
		public int PublishedCountForMonth(int year, int month)
		{
			var firstDate = new DateTime(year, month, day: 1);
			var lastDate = firstDate.AddMonths(1);

			return Connection.GetScalar<int>(@"
				SELECT COUNT(*) 
				FROM blog_posts 
				WHERE published = 1
				AND date BETWEEN {0} AND {1}", firstDate.ToUnix(), lastDate.ToUnix());
		}

		/// <summary>
		/// Get an alphabetical list of available categories
		/// </summary>
		/// <returns>A list of categories</returns>
		public IList<CategoryModel> Categories()
		{
			return Connection.Select<CategoryModel>(query => query.OrderBy(cat => cat.Title)); // Meow
		}

		/// <summary>
		/// Get an alphabetical list of available tags
		/// </summary>
		/// <returns>A list of tags</returns>
		public IList<TagModel> Tags()
		{
			return Connection.Select<TagModel>(query => query.OrderBy(tag => tag.Title));
		}

		/// <summary>
		/// Set the categories this blog post is categorised under
		/// </summary>
		/// <param name="post">The post</param>
		/// <param name="categoryIds">Category IDs</param>
		public void SetCategories(PostSummaryModel post, IEnumerable<int> categoryIds)
		{
			// Find all the currently selected categories
			var current = Connection
				.Select<PostCategoryModel>(x => x.PostId == post.Id)
				.Select(x => x.CategoryId)
				.ToList();

			// Remove items that are in the current list but NOT in the new list
			var toRemove = current.Except(categoryIds).ToList();
			if (toRemove.Count > 0)
				Connection.Delete<PostCategoryModel>("post_id = {0} AND category_id IN ({1})".Params(post.Id, toRemove.SqlInValues()));

			// Add items that are in the new list but NOT in the current list
			var toAdd = categoryIds.Except(current).ToList();
			if (toAdd.Count > 0)
				Connection.InsertAll(toAdd.Select(categoryId => new PostCategoryModel { PostId = post.Id, CategoryId = categoryId }));
		}

		/// <summary>
		/// Set the tags this blog post is tagged with
		/// </summary>
		/// <param name="post">The post</param>
		/// <param name="tagIds">Tag IDs</param>
		public void SetTags(PostSummaryModel post, IEnumerable<int> tagIds)
		{
			// Find all the currently selected tags
			var current = Connection
				.Select<PostTagModel>(x => x.PostId == post.Id)
				.Select(x => x.TagId)
				.ToList();

			// Remove items that are in the current list but NOT in the new list
			var toRemove = current.Except(tagIds).ToList();
			if (toRemove.Count > 0)
				Connection.Delete<PostTagModel>("post_id = {0} AND tag_ids IN ({1})".Params(post.Id, toRemove.SqlInValues()));

			// Add items that are in the new list but NOT in the current list
			var toAdd = tagIds.Except(current).ToList();
			if (toAdd.Count > 0)
				Connection.InsertAll(toAdd.Select(tagId => new PostTagModel { PostId = post.Id, TagId = tagId }));
		}

		public override void Save(PostModel entity)
		{
			// Assume it's new if the ID isn't set yet
			var isNew = entity.Id == 0;

			base.Save(entity);

			// Update the ID
			if (isNew)
				entity.Id = Convert.ToInt32(Connection.GetLastInsertId());
		}

		private class MonthYearCount
		{
			public int Month { get; set; }
			public int Year { get; set; }
			public int Count { get; set; }
		}
	}
}