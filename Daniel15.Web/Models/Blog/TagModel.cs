using ServiceStack.DataAnnotations;

namespace Daniel15.Web.Models.Blog
{
	[Alias("blog_tags")]
	public class TagModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Slug { get; set; }

		[Alias("parent_tag_id")]
		public int? ParentId { get; set; }
	}
}