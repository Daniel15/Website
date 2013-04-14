using ServiceStack.DataAnnotations;

namespace Daniel15.Data.Entities.Blog
{
	/// <summary>
	/// A category in the blog
	/// </summary>
	[Alias("v_blog_categories")]
	public class CategoryModel
	{
		/// <summary>
		/// ID of the category
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Title of the category
		/// </summary>
		public string Title { get; set; }
		/// <summary>
		/// Slug (URL alias) of the category
		/// </summary>
		public string Slug { get; set; }
		/// <summary>
		/// Parent category ID
		/// </summary>
		[Alias("parent_category_id")]
		public int? ParentId { get; set; }
		/// <summary>
		/// Slug (URL alias) of the parent category
		/// </summary>
		[Alias("parent_slug")]
		public string ParentSlug { get; set; }
	}
}