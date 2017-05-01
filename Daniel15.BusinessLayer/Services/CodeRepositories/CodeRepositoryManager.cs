using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Daniel15.BusinessLayer.Services.CodeRepositories
{
	/// <summary>
	/// Handles getting information about a code repository by delegating the request to the correct
	/// <see cref="ICodeRepository"/> implementation.
	/// </summary>
	public class CodeRepositoryManager : ICodeRepositoryManager
	{
		private readonly IEnumerable<ICodeRepository> _codeRepositories;

		public CodeRepositoryManager(IEnumerable<ICodeRepository> codeRepositories)
		{
			_codeRepositories = codeRepositories;
		}

		/// <summary>
		/// Gets information on the specified repository by delegating to the correct 
		/// <see cref="ICodeRepository"/> implementation.
		/// </summary>
		/// <param name="repositoryUrl">URL to the repository</param>
		/// <returns>Information on the repository</returns>
		public async Task<RepositoryInfo> GetRepositoryInfoAsync(Uri repositoryUrl)
		{
			return await FindHandlerFor(repositoryUrl).GetRepositoryInfoAsync(repositoryUrl);
		}

		/// <summary>
		/// Determine the correct <see cref="ICodeRepository"/> to handle the specified repository
		/// </summary>
		/// <param name="repositoryUrl">URL to the repository</param>
		/// <returns>Handler for this repository</returns>
		private ICodeRepository FindHandlerFor(Uri repositoryUrl)
		{
			var handler = _codeRepositories.FirstOrDefault(repo => repo.CanHandle(repositoryUrl));
			if (handler == null)
			{
				throw new Exception("No repository handler available to process '" + repositoryUrl + "'");
			}

			return handler;
		}
	}
}
