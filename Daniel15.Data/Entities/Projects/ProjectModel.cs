using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace Daniel15.Data.Entities.Projects
{
	/// <summary>
	/// Represents a project I've worked on in the past
	/// </summary>
	[Alias("projects")]
	public class ProjectModel
	{
		public string Name { get; set; }
		public string Slug { get; set; }
		public string Url { get; set; }
		public string Thumbnail { get; set; }
		[Alias("thumbnail_height")]
		public int? ThumbnailHeight { get; set; }
		[Alias("thumbnail_width")]
		public int? ThumbnailWidth { get; set; }
		[Alias("type")]
		public ProjectType ProjectType { get; set; }
		public string Description { get; set; }
		public string Date { get; set; }
		[Alias("readme_url")]
		public string ReadmeUrl { get; set; }

		/// <summary>
		/// Whether this project is still being worked on
		/// </summary>
		[Alias("current")]
		public bool IsCurrent { get; set; }

		/// <summary>
		/// A list of technologies used during development of this project
		/// </summary>
		public IList<string> Technologies { get; set; }

		public int Order { get; set; }
	}

	public enum ProjectType
	{
		Website,
		WebApp,
		Library
	}
}