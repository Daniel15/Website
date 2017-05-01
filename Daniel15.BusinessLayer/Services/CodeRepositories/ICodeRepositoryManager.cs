using System;
using System.Threading.Tasks;

namespace Daniel15.BusinessLayer.Services.CodeRepositories
{
	/// <summary>
	/// Handles getting information about a code repository by delegating the request to the correct
	/// <see cref="ICodeRepository"/> implementation.
	/// </summary>
	public interface ICodeRepositoryManager
	{
		/// <summary>
		/// Gets information on the specified repository by delegating to the correct 
		/// <see cref="ICodeRepository"/> implementation.
		/// </summary>
		/// <param name="repositoryUrl">URL to the repository</param>
		/// <returns>Information on the repository</returns>
		Task<RepositoryInfo> GetRepositoryInfoAsync(Uri repositoryUrl);
	}
}
