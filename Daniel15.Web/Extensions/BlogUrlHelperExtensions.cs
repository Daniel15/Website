using System.Web.Mvc;
using Daniel15.Data.Entities.Blog;

namespace Daniel15.Web.Extensions
{
	/// <summary>
	/// URL helpers for blog posts
	/// </summary>
	public static class BlogUrlHelperExtensions
	{
		/// <summary>
		/// Gets a URL to the specified blog post
		/// </summary>
		/// <param name="urlHelper">The URL helper.</param>
		/// <param name="post">Blog post to link to</param>
		/// <returns>URL to this blog post</returns>
		public static string BlogPost(this UrlHelper urlHelper, PostSummaryModel post)
		{
			// Post date needs to be padded with a 0 (eg. "01" for January) - T4MVC doesn't work in this
			// case because it's strongly-typed (can't pass a string for an int param)

			//return urlHelper.Action(MVC.Blog.View(post.Date.Month, post.Date.Year, post.Slug));
			return urlHelper.Action("View", "Blog", new { month = post.Date.Month.ToString("00"), year = post.Date.Year, slug = post.Slug, area = string.Empty });
		}

		/// <summary>
		/// Gets an absolute URL to the specified blog post
		/// </summary>
		/// <param name="urlHelper">The URL helper.</param>
		/// <param name="post">Blog post to link to</param>
		/// <returns>URL to this blog post</returns>
		public static string BlogPostAbsolute(this UrlHelper urlHelper, PostSummaryModel post)
		{
			return urlHelper.Absolute(urlHelper.BlogPost(post));
		}

		/// <summary>
		/// Gets a URL to edit the specified blog post
		/// </summary>
		/// <param name="urlHelper">The URL helper.</param>
		/// <param name="post">Blog post to link to</param>
		/// <returns>URL to edit this blog post</returns>
		public static string BlogPostEdit(this UrlHelper urlHelper, PostSummaryModel post)
		{
			// Post date needs to be padded with a 0 (eg. "01" for January) - T4MVC doesn't work in this
			// case because it's strongly-typed (can't pass a string for an int param)

			//return urlHelper.Action(MVC.Blog.View(post.Date.Month, post.Date.Year, post.Slug));
			return urlHelper.Action("Edit", "Blog", new { area = "Admin", month = post.Date.Month.ToString("00"), year = post.Date.Year, slug = post.Slug });
		}

		/// <summary>
		/// Gets the URL to the index of the blog
		/// </summary>
		/// <param name="urlHelper">The URL helper</param>
		/// <param name="page">Page number to link to</param>
		/// <returns>The URL</returns>
		public static string BlogIndex(this UrlHelper urlHelper, int page = 1)
		{
			return page == 1 
				? urlHelper.RouteUrl("BlogHome") 
				: urlHelper.RouteUrl("BlogHomePage", new { page = page });
		}

		/// <summary>
		/// Gets the URL to the specified blog tagged post listing
		/// </summary>
		/// <param name="urlHelper">The URL helper</param>
		/// <param name="category">Category to link to</param>
		/// <param name="page">Page number to link to</param>
		/// <returns>The URL</returns>
		public static string BlogCategory(this UrlHelper urlHelper, CategoryModel category, int page = 1)
		{
			return urlHelper.BlogCategory(category.Slug, page);
		}

		/// <summary>
		/// Gets the URL to the specified blog category
		/// </summary>
		/// <param name="urlHelper">The URL helper</param>
		/// <param name="slug">Slug of the category</param>
		/// <param name="page">Page number to link to</param>
		/// <returns>The URL</returns>
		public static string BlogCategory(this UrlHelper urlHelper, string slug, int page = 1)
		{
			return page == 1
				? urlHelper.RouteUrl("BlogCategory", new { slug })
				: urlHelper.RouteUrl("BlogCategoryPage", new { slug, page });
		}

		/// <summary>
		/// Gets the URL to the specified blog tagged post listing
		/// </summary>
		/// <param name="urlHelper">The URL helper</param>
		/// <param name="tag">Tag to link to</param>
		/// <param name="page">Page number to link to</param>
		/// <returns>The URL</returns>
		public static string BlogTag(this UrlHelper urlHelper, TagModel tag, int page = 1)
		{
			return urlHelper.BlogTag(tag.Slug, page);
		}

		/// <summary>
		/// Gets the URL to the specified blog tagged post listing
		/// </summary>
		/// <param name="urlHelper">The URL helper</param>
		/// <param name="slug">Slug of the tag</param>
		/// <param name="page">Page number to link to</param>
		/// <returns>The URL</returns>
		public static string BlogTag(this UrlHelper urlHelper, string slug, int page = 1)
		{
			return page == 1
				? urlHelper.RouteUrl("BlogTag", new { slug })
				: urlHelper.RouteUrl("BlogTagPage", new { slug, page });
		}
	}
}