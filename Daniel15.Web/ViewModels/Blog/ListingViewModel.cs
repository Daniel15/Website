using System;
using System.Collections.Generic;
using Daniel15.Web.Models.Blog;

namespace Daniel15.Web.ViewModels.Blog
{
	/// <summary>
	/// Data for a blog listing page
	/// </summary>
	public class ListingViewModel : ViewModelBase
	{
		/// <summary>
		/// Posts to display on the current blog page
		/// </summary>
		public IEnumerable<PostViewModel> Posts { get; set; }

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
		/// Function used to get the paging URLs
		/// </summary>
		public Func<int, string> PageUrlGenerator { get; set; }
	}
}