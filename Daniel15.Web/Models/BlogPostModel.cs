namespace Daniel15.Web.Models
{
	/// <summary>
	/// Represents a blog post
	/// </summary>
	public class BlogPostModel : BlogPostSummaryModel
	{
		public string Content { get; set; }
	}
}