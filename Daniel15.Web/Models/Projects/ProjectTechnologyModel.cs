namespace Daniel15.Web.Models.Projects
{
	/// <summary>
	/// Represents a technology used in one of my projects
	/// </summary>
	public class ProjectTechnologyModel
	{
		public int Id { get; set; }

		public string Slug { get; set; }
		public string Name { get; set; }
		public string Url { get; set; }
		public string Description { get; set; }
		public int Order { get; set; }

		/// <summary>
		/// Whether this technology is "important" (prominently displayed on the site) or not.
		/// </summary>
		public bool IsPrimary { get; set; }
	}
}
