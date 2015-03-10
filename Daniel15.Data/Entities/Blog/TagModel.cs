using System.Collections.Generic;

namespace Daniel15.Data.Entities.Blog
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
		public virtual ICollection<PostModel> Posts { get; set; }
	}
}