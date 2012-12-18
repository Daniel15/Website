using System;

namespace Daniel15.Web.ViewModels.Shared
{
	public class PaginationModel
	{
		/// <summary>
		/// Page number we're currently on
		/// </summary>
		public int CurrentPage { get; set; }
		/// <summary>
		/// Total number of pages
		/// </summary>
		public int TotalPages { get; set; }
		/// <summary>
		/// Function to generate URLs for paging
		/// </summary>
		public Func<int, string> UrlGenerator { get; set; }
	}
}