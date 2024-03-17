namespace Daniel15.Web.Models.Blog;

/**
 * Represents the front matter on Markdown blog posts
 */
public class PostFrontMatter
{
	public int Id { get; set; }

	public string Title { get; set; }

	public bool Published { get; set; }

	public DateTime PublishedDate { get; set; }

	public DateTime LastModifiedDate { get; set; }

	public string Summary { get; set; }

	public IList<string> Categories { get; set; }
}
