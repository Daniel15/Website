namespace Daniel15.Data.Entities.Blog
{
	/// <summary>
	/// Many-to-many entity between <see cref="TagModel"/> and <see cref="PostModel"/>
	/// </summary>
	public class PostTagModel
	{
		public int TagId { get; set; }
		public TagModel Tag { get; set; }

		public int PostId { get; set; }
		public PostModel Post { get; set; }
	}
}
