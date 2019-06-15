using System;
using System.Collections.Generic;
using Daniel15.Shared.Extensions;

namespace Daniel15.Data.Entities.Projects
{
	/// <summary>
	/// Represents a project I've worked on in the past
	/// </summary>
	public class ProjectModel
	{
		/// <summary>
		/// Database ID for this project
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Name of this project
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// URL alias ("slug") for this project
		/// </summary>
		public string Slug { get; set; }

		/// <summary>
		/// Live URL to this project, or <c>null</c> if it's no longer online
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// Thumbnail image name for this project
		/// </summary>
		public string Thumbnail { get; set; }

		/// <summary>
		/// Height of the thumbnail, or <c>null</c> for the default
		/// </summary>
		public int? ThumbnailHeight { get; set; }

		/// <summary>
		/// Width of the thumbnail, or <c>null</c> for the default
		/// </summary>
		public int? ThumbnailWidth { get; set; }

		/// <summary>
		/// Project type
		/// </summary>
		public ProjectType ProjectType { get; set; }

		/// <summary>
		/// Description of this project
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Date this project was created or last modified. Free-form text, can be anything (eg. 
		/// "September 2013" or "Always updated")
		/// </summary>
		public string Date { get; set; }

		/// <summary>
		/// URL to a Markdown readme file for this project
		/// </summary>
		public string ReadmeUrl { get; set; }

		/// <summary>
		/// Cached text of the readme file
		/// </summary>
		public string Readme { get; set; }

		/// <summary>
		/// Whether this project is still being worked on
		/// </summary>
		public bool IsCurrent { get; set; }

		/// <summary>
		/// A list of technologies used during development of this project
		/// </summary>
		public IList<string> Technologies { get; set; }

		/// <summary>
		/// A number representing where to display this project in the list
		/// </summary>
		public int Order { get; set; }

		#region Repository information
		/// <summary>
		/// Code repository for this project, if it's open-source
		/// </summary>
		public string RepositoryUrl { get; set; }

		/// <summary>
		/// Date this project was created
		/// </summary>
		public DateTime? Created { get; set; }

		/// <summary>
		/// Date this project was updated
		/// </summary>
		public DateTime? Updated { get; set; }

		/// <summary>
		/// Number of forks this project has.
		/// </summary>
		public int? Forks { get; set; }

		/// <summary>
		/// Number of "watchers" (users watching it) this project has.
		/// </summary>
		public int? Watchers { get; set; }

		/// <summary>
		/// Number of bugs or issues this project has.
		/// </summary>
		public int? OpenIssues { get; set; }
		#endregion
	}

	/// <summary>
	/// Represents a project type
	/// </summary>
	public enum ProjectType
	{
		/// <summary>
		/// This project is a regular web site
		/// </summary>
		Website,

		/// <summary>
		/// This project is a web application
		/// </summary>
		Webapp,

		/// <summary>
		/// This project is a reusable library
		/// </summary>
		Library
	}
}
