namespace Daniel15.Web.Models.Home
{
	/// <summary>
	/// Represents a technology used in one of my projects
	/// </summary>
	public class ProjectTechnologyModel
	{
		public string Alias { get; set; }
		public string Name { get; set; }
		public string Url { get; set; }
		public string Desc { get; set; }

		/// <summary>
		/// Whether this technology is "important" (prominently displayed on the site) or not.
		/// </summary>
		public bool IsPrimary { get; set; }
	}
}