using ServiceStack.DataAnnotations;

namespace Daniel15.Data.Entities.Blog
{
	[Alias("blog_categories")]
	public class CategoryModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Slug { get; set; }

		[Alias("parent_category_id")]
		public int? ParentId { get; set; }
	}
}