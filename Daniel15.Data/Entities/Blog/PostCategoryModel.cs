using ServiceStack.DataAnnotations;

namespace Daniel15.Data.Entities.Blog
{
	/// <summary>
	/// The link between a post and a category
	/// </summary>
	[Alias("blog_post_categories")]
	public class PostCategoryModel
	{
		[Alias("post_id")]
		public int PostId { get; set; }

		[Alias("category_id")]
		public int CategoryId { get; set; }
	}
}