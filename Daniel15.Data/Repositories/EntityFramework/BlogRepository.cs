using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Daniel15.Data.Entities.Blog;
using Daniel15.Data.Extensions;
using Daniel15.Shared.Extensions;

namespace Daniel15.Data.Repositories.EntityFramework
{
	/// <summary>
	/// Blog repository that uses Entity Framework as the data access component
	/// </summary>
	public class BlogRepository : RepositoryBase<PostModel>, IBlogRepository
	{
		/// <summary>
		/// Creates a new <see cref="BlogRepository"/>.
		/// </summary>
		/// <param name="context"></param>
		public BlogRepository(DatabaseContext context) : base(context) {}

		/// <summary>
		/// Gets the <see cref="DbSet{T}"/> represented by this repository.
		/// </summary>
		protected override DbSet<PostModel> Set
		{
			get { return Context.Posts; }
		}

		/// <summary>
		/// Gets a post by slug.
		/// </summary>
		/// <param name="slug">The slug.</param>
		/// <returns>The post</returns>
		public PostModel GetBySlug(string slug)
		{
			return Context.Posts
				.Include(post => post.MainCategory)
				.Include(post => post.MainCategory.Parent)
				.FirstOrThrow(post => post.Slug == slug);
		}

		/// <summary>
		/// Gets the categories for the specified blog post
		/// </summary>
		/// <param name="post">Blog post</param>
		/// <returns>Categories for this blog post</returns>
		public IList<CategoryModel> CategoriesForPost(PostModel post)
		{
			return post == null || post.Id == 0
				? new List<CategoryModel>() 
				: Context.Posts.Find(post.Id).Categories.ToList();
		}

		/// <summary>
		/// Gets the categories for the specified blog posts
		/// </summary>
		/// <param name="posts">Blog posts</param>
		/// <returns>Categories for all the specified posts</returns>
		public IDictionary<PostModel, IEnumerable<CategoryModel>> CategoriesForPosts(IEnumerable<PostModel> posts)
		{
			if (!posts.Any())
				return new Dictionary<PostModel, IEnumerable<CategoryModel>>();

			var postIDs = posts.Select(post => post.Id);

			// This is optimised to only perform a single query to get all categories
			var categories = Context.Categories
				.Where(cat => cat.Posts.Any(post => postIDs.Contains(post.Id)))
				.Select(cat => new
				{
					Category = cat,
					PostIDs = cat.Posts.Select(post => post.Id)
				});

			// Group the categories by post ID
			// TODO: There's almost certainly a better way of doing this...
			var postIDToCategory = new Dictionary<int, IList<CategoryModel>>();
			foreach (var categoryAndPost in categories)
			{
				foreach (var postId in categoryAndPost.PostIDs)
				{
					if (!postIDToCategory.ContainsKey(postId))
					{
						postIDToCategory.Add(postId, new List<CategoryModel>());
                    }
					postIDToCategory[postId].Add(categoryAndPost.Category);
				}
			}

			return posts.ToDictionary(
				post => post,
				post => postIDToCategory[post.Id].AsEnumerable()
			);
		}

		/// <summary>
		/// Gets the tags for the specified blog post
		/// </summary>
		/// <param name="post">Blog post</param>
		/// <returns>Tags for this blog post</returns>
		public IList<TagModel> TagsForPost(PostModel post)
		{
			return post == null || post.Id == 0 
				? new List<TagModel>()
				: Context.Posts.Find(post.Id).Tags.ToList();
		}

		/// <summary>
		/// Gets the latest blog posts
		/// </summary>
		/// <param name="count">Number of posts to return</param>
		/// <param name="offset">Post to start at</param>
		/// <param name="published">Whether to return published or unpublished posts</param>
		/// <returns>Latest blog posts</returns>
		public List<PostModel> LatestPosts(int count = 10, int offset = 0, bool published = true)
		{
			return LatestPosts(
				Context.Posts,
				count,
				offset,
				published
			);

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
			return LatestPosts(category.Posts.AsQueryable(), count, offset);
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
			return LatestPosts(tag.Posts.AsQueryable(), count, offset);
		}

		/// <summary>
		/// Gets the latest blog posts from the specified queryable
		/// </summary>
		/// <param name="posts">Data source for blog posts</param>
		/// <param name="count">Number of posts to return</param>
		/// <param name="offset">Post to start at</param>
		/// <returns>Latest blog posts</returns>
		private List<PostModel> LatestPosts(IQueryable<PostModel> posts, int count, int offset, bool published = true)
		{
			return posts
				.Where(post => post.Published == published)
				.OrderByDescending(post => post.UnixDate)
				.Skip(offset)
				.Take(count)
				.Include(post => post.MainCategory)
				.Include(post => post.MainCategory.Parent)
				.ToList();
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
			var firstDate = new DateTime(year, month, day: 1).ToUnix();
			var lastDate = new DateTime(year, month, day: 1).AddMonths(1).ToUnix();

			return Context.Posts
				.Where(post =>
					post.Published &&
					post.UnixDate >= firstDate &&
					post.UnixDate <= lastDate
				)
				.OrderByDescending(post => post.UnixDate)
				.Skip(offset)
				.Take(count)
				.Include(post => post.MainCategory)
				.ToList();
		}

		/// <summary>
		/// Gets the count of blog posts for every year and every month.
		/// </summary>
		/// <returns>A dictionary of years, which contains a dictionary of months and counts</returns>
		public IDictionary<int, IDictionary<int, int>> MonthCounts()
		{
			var counts = Context.Database.SqlQuery<MonthYearCount>(@"
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
			return Context.Categories
				.Include(cat => cat.Parent)
				.FirstOrThrow(x => x.Slug == slug);
		}

		/// <summary>
		/// Gets a tag by slug
		/// </summary>
		/// <param name="slug">Slug of the tag</param>
		/// <returns>The tag</returns>
		public TagModel GetTag(string slug)
		{
			return Context.Tags.FirstOrThrow(x => x.Slug == slug);
		}

		/// <summary>
		/// Get the total number of posts that are published
		/// </summary>
		/// <returns>Total number of posts</returns>
		public int PublishedCount()
		{
			return Context.Posts.Count(post => post.Published);
		}

		/// <summary>
		/// Get the total number of posts that are published in this category
		/// </summary>
		/// <returns>Total number of posts in the category</returns>
		public int PublishedCount(CategoryModel category)
		{
			return category.Posts.Count(post => post.Published);
		}

		/// <summary>
		/// Get the total number of posts that are published in this tag
		/// </summary>
		/// <returns>Total number of posts in the tag</returns>
		public int PublishedCount(TagModel tag)
		{
			return tag.Posts.Count(post => post.Published);
		}

		/// <summary>
		/// Get the total number of posts that are not yet published
		/// </summary>
		/// <returns>Total number of posts that have not yet been published</returns>
		public int UnpublishedCount()
		{
			return Context.Posts.Count(post => !post.Published);
		}

		/// <summary>
		/// Get the total number of posts that are published in this month and year
		/// </summary>
		/// <param name="year">Year to get count for</param>
		/// <param name="month">Month to get count for</param>
		/// <returns>Total number of posts that were posted in this month</returns>
		public int PublishedCountForMonth(int year, int month)
		{
			var firstDate = new DateTime(year, month, day: 1).ToUnix();
			var lastDate = new DateTime(year, month, day: 1).AddMonths(1).ToUnix();

			return Context.Posts.Count(post =>
				post.Published && 
				post.UnixDate >= firstDate &&
				post.UnixDate <= lastDate
			);
		}

		/// <summary>
		/// Get an alphabetical list of available categories
		/// </summary>
		/// <returns>A list of categories</returns>
		public IList<CategoryModel> Categories()
		{
			return Context.Categories
				.OrderBy(cat => cat.Title) // Meow
				.ToList();
		}

		/// <summary>
		/// Get an alphabetical list of categories that contain posts
		/// </summary>
		/// <returns>A list of categories</returns>
		public IList<CategoryModel> CategoriesInUse()
		{
			return Context.Categories
				.Where(cat => cat.Posts.Count > 0)
				.OrderBy(cat => cat.Title)
				.ToList();
		}

		/// <summary>
		/// Get an alphabetical list of available tags
		/// </summary>
		/// <returns>A list of tags</returns>
		public IList<TagModel> Tags()
		{
			return Context.Tags
				.OrderBy(tag => tag.Title)
				.ToList();
		}

		/// <summary>
		/// Set the categories this blog post is categorised under
		/// </summary>
		/// <param name="post">The post</param>
		/// <param name="categoryIds">Category IDs</param>
		public void SetCategories(PostModel post, IEnumerable<int> categoryIds)
		{
			var newCategories = Context.Categories.Where(cat => categoryIds.Contains(cat.Id));
			// Entity Framework is smart enough to work out the adds/deletes here.
			post.Categories.Clear();
			foreach (var category in newCategories)
			{
				post.Categories.Add(category);
			}
			Context.SaveChanges();
		}

		/// <summary>
		/// Set the tags this blog post is tagged with
		/// </summary>
		/// <param name="post">The post</param>
		/// <param name="tagIds">Tag IDs</param>
		public void SetTags(PostModel post, IEnumerable<int> tagIds)
		{
			var newTags = Context.Tags.Where(tag => tagIds.Contains(tag.Id));
			post.Tags.Clear();
			foreach (var tag in newTags)
			{
				post.Tags.Add(tag);
			}
			Context.SaveChanges();
		}

		private class MonthYearCount
		{
			public int Month { get; set; }
			public int Year { get; set; }
			public int Count { get; set; }
		}
	}
}
