using Daniel15.Data.Entities.Blog;

namespace Daniel15.Web.ViewModels.Blog
{
	/// <summary>
	/// Data for a tag blog listing page
	/// </summary>
	public class TagListingViewModel : ListingViewModel
	{
		public TagModel Tag { get; set; }
	}
}