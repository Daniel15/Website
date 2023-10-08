using System;

namespace Daniel15.Web.Services.CodeRepositories
{
	// TODO: Move to DTO assembly
	/// <summary>
	/// Represents information on a code repository
	/// </summary>
	public class RepositoryInfo
	{
		/// <summary>
		/// Number of forks the repository has.
		/// </summary>
		public int? Forks { get; set; }

		/// <summary>
		/// Number of "watchers" (users watching it) the repository has.
		/// </summary>
		public int? Watchers { get; set; }

		/// <summary>
		/// Number of bugs or issues the repository has.
		/// </summary>
		public int? OpenIssues { get; set; }

		/// <summary>
		/// Date the repository was created
		/// </summary>
		public DateTime? Created { get; set; }

		/// <summary>
		/// Date the repository was last updated
		/// </summary>
		public DateTime? Updated { get; set; }
	}
}
