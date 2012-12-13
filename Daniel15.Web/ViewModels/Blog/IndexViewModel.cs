using System;
using System.Collections.Generic;
using Daniel15.Web.Models.Blog;

namespace Daniel15.Web.ViewModels.Blog
{
	public class IndexViewModel : ViewModelBase
	{
		/// <summary>
		/// Posts to display on the current blog page
		/// </summary>
		public IList<PostModel> Posts { get; set; }

		/// <summary>
		/// Total number of posts that are available
		/// </summary>
		public int TotalCount { get; set; }

		/// <summary>
		/// Current page number
		/// </summary>
		public int Page { get; set; }

		/// <summary>
		/// Total number of pages
		/// </summary>
		public int TotalPages { get; set; }

		/// <summary>
		/// Function used to shorten blog URLs
		/// TODO: Is this the best way to do this?
		/// </summary>
		public Func<PostModel, string> UrlShortener { get; set; }
	}
}