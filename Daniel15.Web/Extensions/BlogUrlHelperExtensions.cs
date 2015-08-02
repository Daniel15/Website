using Daniel15.Data.Entities.Blog;
using Microsoft.AspNet.Mvc;

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
		public static string BlogPost(this IUrlHelper urlHelper, PostModel post)
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
		public static string BlogPostAbsolute(this IUrlHelper urlHelper, PostModel post)
		{
			return urlHelper.Absolute(urlHelper.BlogPost(post));
		}

		/// <summary>
		/// Gets a URL to edit the specified blog post
		/// </summary>
		/// <param name="urlHelper">The URL helper.</param>
		/// <param name="post">Blog post to link to</param>
		/// <returns>URL to edit this blog post</returns>
		public static string BlogPostEdit(this IUrlHelper urlHelper, PostModel post)
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
		public static string BlogIndex(this IUrlHelper urlHelper, int page = 1)
		{
			return page == 1 
				? urlHelper.RouteUrl("BlogHome") 
				: urlHelper.RouteUrl("BlogHomePage", new { page = page });
		}
		
		/// <summary>
		/// Gets the URL to the specified blog category post listing
		/// </summary>
		/// <param name="urlHelper">The URL helper</param>
		/// <param name="category">Category to link to</param>
		/// <param name="page">Page number to link to</param>
		/// <returns>The URL</returns>
		public static string BlogCategory(this IUrlHelper urlHelper, CategoryModel category, int page = 1)
		{
			return BlogCategory(urlHelper, "Category", category.Slug, category.Parent == null ? null : category.Parent.Slug, page);
		}

		/// <summary>
		/// Gets the URL to the specified blog category RSS feed
		/// </summary>
		/// <param name="urlHelper">The URL helper</param>
		/// <param name="category">Category to link to</param>
		/// <returns>The URL</returns>
		public static string BlogCategoryFeed(this IUrlHelper urlHelper, CategoryModel category)
		{
			return BlogCategory(urlHelper, "CategoryFeed", category.Slug, category.Parent == null ? null :  category.Parent.Slug);
		}

		/// <summary>
		/// Gets the URL to the specified blog category post listing
		/// </summary>
		/// <param name="urlHelper">The URL helper</param>
		/// <param name="parentSlug">Slug of the parent</param>
		/// <param name="page">Page number to link to</param>
		/// <param name="routeType">Route type to link to (eg. "Category" or "CategoryFeed")</param>
		/// <param name="slug">Slug of the category</param>
		/// <returns>The URL</returns>
		public static string BlogCategory(this IUrlHelper urlHelper, string routeType, string slug, string parentSlug, int page = 1)
		{
			// It's safer to explicitly use the correct route here, instead of relying on the ASP.NET 
			// routing engine to choose it.
			if (string.IsNullOrEmpty(parentSlug))
			{
				return page == 1
					? urlHelper.RouteUrl("Blog" + routeType, new { slug = slug })
					: urlHelper.RouteUrl("Blog" + routeType + "Page", new { slug = slug, page = page });
			}
			else
			{
				return page == 1
					? urlHelper.RouteUrl("BlogSub" + routeType, new { slug = slug, parentSlug = parentSlug })
					: urlHelper.RouteUrl("BlogSub" + routeType + "Page", new { slug = slug, parentSlug = parentSlug, page = page });
			}
		}

		/// <summary>
		/// Gets the URL to the specified blog tagged post listing
		/// </summary>
		/// <param name="urlHelper">The URL helper</param>
		/// <param name="tag">Tag to link to</param>
		/// <param name="page">Page number to link to</param>
		/// <returns>The URL</returns>
		public static string BlogTag(this IUrlHelper urlHelper, TagModel tag, int page = 1)
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
		public static string BlogTag(this IUrlHelper urlHelper, string slug, int page = 1)
		{
			return page == 1
				? urlHelper.RouteUrl("BlogTag", new { slug })
				: urlHelper.RouteUrl("BlogTagPage", new { slug, page });
		}
	}
}