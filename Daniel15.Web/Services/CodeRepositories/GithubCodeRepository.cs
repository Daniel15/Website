using Daniel15.Web.Extensions;

namespace Daniel15.Web.Services.CodeRepositories
{
	public class GithubCodeRepository : ICodeRepository
	{
		/// <summary>
		/// Base URL to the Github API
		/// </summary>
		private const string API_BASE = "https://api.github.com/";

		/// <summary>
		/// URL to the Github API endpoint to get repository details
		/// </summary>
		private const string API_REPO = API_BASE + "repos/{0}/{1}";

		private readonly HttpClient _client;

		public GithubCodeRepository(HttpClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Determines whether this implementation can handle the specified repository
		/// </summary>
		/// <param name="repositoryUrl">URL to the repository</param>
		/// <returns><c>true</c> if this implementation can handle it</returns>
		public bool CanHandle(Uri repositoryUrl)
		{
			return IsGithubRepository(repositoryUrl);
		}

		/// <summary>
		/// Determines whether the specified repository is a Github repository.
		/// </summary>
		/// <param name="repositoryUrl">URL to the repository</param>
		/// <returns><c>true</c> if this is a Github repository</returns>
		public static bool IsGithubRepository(Uri repositoryUrl)
		{
			return (repositoryUrl.Scheme.Equals("git", StringComparison.InvariantCultureIgnoreCase) ||
			        repositoryUrl.Scheme.Equals("http", StringComparison.InvariantCultureIgnoreCase)) &&
				    repositoryUrl.Host.Equals("github.com", StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Gets information on the specified repository
		/// </summary>
		/// <param name="repositoryUrl">URL to the repository</param>
		/// <returns>Information on the repository</returns>
		public async Task<RepositoryInfo> GetRepositoryInfoAsync(Uri repositoryUrl)
		{
			// Split repository URI into username and repository
			// Also trim the ".git" off the end.
			var uriPieces = repositoryUrl.AbsolutePath.TrimStart('/').Replace(".git", string.Empty).Split('/');
			var user = uriPieces[0];
			var repos = uriPieces[1];

			// Get repository details via API
			var apiUrl = string.Format(API_REPO, user, repos);

			var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
			request.Headers.Add("User-Agent", "Daniel15-Website/1.0 (http://dan.cx/)");

			var rawResponse = await _client.SendAsync(request);
			dynamic response = await rawResponse.Content.ReadAsDynamicJsonAsync();
			return new RepositoryInfo
			{
				Created = response.created_at,
				Updated = response.updated_at,
				Forks = response.forks,
				Watchers = response.watchers,
				OpenIssues = response.open_issues,
			};
		}
	}
}
