namespace Daniel15.Web.Models.Blog
{
	public class TagModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Slug { get; set; }
		public int? ParentId { get; set; }

		/// <summary>
		/// Posts that are tagged with this tag
		/// </summary>
		public virtual List<PostTagModel> PostTags { get; set; }
	}
}
