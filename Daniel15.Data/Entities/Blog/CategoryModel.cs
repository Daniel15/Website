using System.Collections.Generic;

namespace Daniel15.Data.Entities.Blog
{
	/// <summary>
	/// A category in the blog
	/// </summary>
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
		public int? ParentId { get; set; }
		/// <summary>
		/// Parent category this category is contained in
		/// </summary>
		public virtual CategoryModel Parent { get; set; }
		/// <summary>
		/// Posts that belong to this category
		/// </summary>
		public virtual ICollection<PostModel> Posts { get; set; }
	}
}