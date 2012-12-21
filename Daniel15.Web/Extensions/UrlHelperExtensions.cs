using System;
using System.Web.Mvc;
using Daniel15.Web.Models.Blog;

namespace Daniel15.Web.Extensions
{
	/// <summary>
	/// Various URL helpers
	/// </summary>
	public static class UrlHelperExtensions
	{
		/// <summary>
		/// Gets the URL to the specified JavaScript file
		/// </summary>
		/// <param name="urlHelper">The URL helper.</param>
		/// <param name="filename">The filename.</param>
		/// <returns>JavaScript URL</returns>
		public static string Js(this UrlHelper urlHelper, string filename)
		{
			return urlHelper.Content("~/Content/js/" + filename);
		}

		/// <summary>
		/// Gets the URL to the specified image
		/// </summary>
		/// <param name="urlHelper">The URL helper.</param>
		/// <param name="filename">The filename.</param>
		/// <returns>Image URL</returns>
		public static string Image(this UrlHelper urlHelper, string filename)
		{
			return urlHelper.Content("~/Content/images/" + filename);
		}

		/// <summary>
		/// Converts the specified relative URI into an absolute URL
		/// </summary>
		/// <param name="urlHelper">URL helper</param>
		/// <param name="uri">Relative URI</param>
		/// <returns>Absoute URL</returns>
		public static string Absolute(this UrlHelper urlHelper, string uri)
		{
			return urlHelper.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) + uri;
		}

		/// <summary>
		/// Gets the absolute URL (http://..../blah) to the specified URI
		/// </summary>
		/// <param name="urlHelper">The URL helper</param>
		/// <param name="uri">URI to link to</param>
		/// <returns>The absolute URL</returns>
		public static string ContentAbsolute(this UrlHelper urlHelper, string uri)
		{
			return urlHelper.Absolute(urlHelper.Content(uri));
		}

		/// <summary>
		/// Gets a URL to the specified blog post
		/// </summary>
		/// <param name="urlHelper">The URL helper.</param>
		/// <param name="post">Blog post to link to</param>
		/// <returns>URL to this blog post</returns>
		public static string Blog(this UrlHelper urlHelper, PostSummaryModel post)
		{
			// Post date needs to be padded with a 0 (eg. "01" for January) - T4MVC doesn't work in this
			// case because it's strongly-typed (can't pass a string for an int param)

			//return urlHelper.Action(MVC.Blog.View(post.Date.Month, post.Date.Year, post.Slug));
			return urlHelper.Action("View", "Blog", new { month = post.Date.Month.ToString("00"), year = post.Date.Year, slug = post.Slug });
		}

		/// <summary>
		/// Gets an absolute URL to the specified blog post
		/// </summary>
		/// <param name="urlHelper">The URL helper.</param>
		/// <param name="post">Blog post to link to</param>
		/// <returns>URL to this blog post</returns>
		public static string BlogAbsolute(this UrlHelper urlHelper, PostSummaryModel post)
		{
			return urlHelper.Absolute(urlHelper.Blog(post));
		}

		/// <summary>
		/// Gets a URL to edit the specified blog post
		/// </summary>
		/// <param name="urlHelper">The URL helper.</param>
		/// <param name="post">Blog post to link to</param>
		/// <returns>URL to edit this blog post</returns>
		public static string BlogEdit(this UrlHelper urlHelper, PostSummaryModel post)
		{
			// Post date needs to be padded with a 0 (eg. "01" for January) - T4MVC doesn't work in this
			// case because it's strongly-typed (can't pass a string for an int param)

			//return urlHelper.Action(MVC.Blog.View(post.Date.Month, post.Date.Year, post.Slug));
			return urlHelper.Action("Edit", "Blog", new { area = "Admin", month = post.Date.Month.ToString("00"), year = post.Date.Year, slug = post.Slug });
		}
	}
}