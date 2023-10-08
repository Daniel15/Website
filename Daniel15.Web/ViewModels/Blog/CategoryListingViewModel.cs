
using Daniel15.Web.Models.Blog;

namespace Daniel15.Web.ViewModels.Blog
{
	/// <summary>
	/// Data for a category blog listing page
	/// </summary>
	public class CategoryListingViewModel : ListingViewModel
	{
		public CategoryModel Category { get; set; }

		public string RssUrl { get; set; }
	}
}
