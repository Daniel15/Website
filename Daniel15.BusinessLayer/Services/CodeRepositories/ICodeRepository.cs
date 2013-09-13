using System;

namespace Daniel15.BusinessLayer.Services.CodeRepositories
{
	/// <summary>
	/// Handles getting information about a code repository from a specific provider (for example, GitHub)
	/// </summary>
	public interface ICodeRepository
	{
		/// <summary>
		/// Determines whether this implementation can handle the specified repository
		/// </summary>
		/// <param name="repositoryUrl">URL to the repository</param>
		/// <returns><c>true</c> if this implementation can handle it</returns>
		bool CanHandle(Uri repositoryUrl);

		/// <summary>
		/// Gets information on the specified repository
		/// </summary>
		/// <param name="repositoryUrl">URL to the repository</param>
		/// <returns>Information on the repository</returns>
		RepositoryInfo GetRepositoryInfo(Uri repositoryUrl);
	}
}
