using ServiceStack.DataAnnotations;

namespace Daniel15.Data.Entities.Blog
{
	/// <summary>
	/// Category model with added post ID. This is used when retrieving categories for posts and 
	/// should not be exposed outside this assembly.
	/// </summary>
	internal class CategoryWithPostIdModel : CategoryModel
	{
		/// <summary>
		/// ID of the post this category is associated with.
		/// </summary>
		[Alias("post_id")]
		public int PostId { get; set; }
	}
}
