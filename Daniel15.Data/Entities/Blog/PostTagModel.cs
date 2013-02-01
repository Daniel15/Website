using ServiceStack.DataAnnotations;

namespace Daniel15.Data.Entities.Blog
{
	/// <summary>
	/// The link between a post and a tag
	/// </summary>
	[Alias("blog_post_tags")]
	public class PostTagModel
	{
		[Alias("post_id")]
		public int PostId { get; set; }

		[Alias("tag_id")]
		public int TagId { get; set; }
	}
}