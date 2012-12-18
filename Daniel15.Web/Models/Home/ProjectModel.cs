using System.Collections.Generic;

namespace Daniel15.Web.Models.Home
{
	/// <summary>
	/// Represents a project I've worked on in the past
	/// </summary>
	public class ProjectModel
	{
		public string Name { get; set; }
		public string Url { get; set; }
		public string Thumbnail { get; set; }
		public int? ThumbnailHeight { get; set; }
		public int? ThumbnailWidth { get; set; }
		public ProjectType ProjectType { get; set; }
		public string Description { get; set; }
		public string Date { get; set; }

		/// <summary>
		/// Whether this project is still being worked on
		/// </summary>
		public bool IsCurrent { get; set; }

		/// <summary>
		/// A list of technologies used during development of this project
		/// </summary>
		public IList<ProjectTechnologyModel> Technologies { get; set; }
	}

	public enum ProjectType
	{
		Website,
		WebApplication,
		Library
	}
}