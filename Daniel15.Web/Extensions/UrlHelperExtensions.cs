using System.Web.Mvc;
using Daniel15.Web.Models;

namespace Daniel15.Web.Extensions
{
	/// <summary>
	/// Various URL helpers
	/// </summary>
	public static class UrlHelperExtensions
	{
		/// <summary>
		/// Gets a JavaScrupt URL
		/// </summary>
		/// <param name="urlHelper">The URL helper.</param>
		/// <param name="filename">The filename.</param>
		/// <returns>JavaScript URL</returns>
		public static string Js(this UrlHelper urlHelper, string filename)
		{
			return urlHelper.Content("~/Content/js/" + filename);
		}

		/// <summary>
		/// Returns a URL to the specified blog post
		/// </summary>
		/// <param name="urlHelper">The URL helper.</param>
		/// <param name="post">Blog post to link to</param>
		/// <returns>URL to this blog post</returns>
		public static string Blog(this UrlHelper urlHelper, BlogPostSummaryModel post)
		{
			// Post date needs to be padded with a 0 (eg. "01" for January) - T4MVC doesn't work in this
			// case because it's strongly-typed (can't pass a string for an int param)

			//return urlHelper.Action(MVC.Blog.View(post.Date.Month, post.Date.Year, post.Slug));
			return urlHelper.Action("View", "Blog", new { month = post.Date.Month.ToString("00"), year = post.Date.Year, slug = post.Slug });
		}
	}
}