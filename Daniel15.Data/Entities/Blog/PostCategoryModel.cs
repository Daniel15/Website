namespace Daniel15.Data.Entities.Blog
{
	/// <summary>
	/// Many-to-many entity between <see cref="CategoryModel"/> and <see cref="PostModel"/>
	/// </summary>
    public class PostCategoryModel
    {
		public int CategoryId { get; set; }
		public CategoryModel Category { get; set; }

		public int PostId { get; set; }
		public PostModel Post { get; set; }
    }
}
