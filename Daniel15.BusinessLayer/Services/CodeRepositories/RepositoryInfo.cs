using System;

namespace Daniel15.BusinessLayer.Services.CodeRepositories
{
	// TODO: Move to DTO assembly
	/// <summary>
	/// Represents information on a code repository
	/// </summary>
	public class RepositoryInfo
	{
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
