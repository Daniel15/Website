using Coravel.Invocable;
using Daniel15.Web.Models.Blog;
using Daniel15.Web.Repositories;
using Daniel15.Web.Configuration;
using Daniel15.Web.Extensions;
using Microsoft.AspNetCore.Http.Extensions;

namespace Daniel15.Web.Services
{
	/// <summary>
	/// Handles synchronisation of comments between Disqus and the local database.
	/// </summary>
	public class DisqusComments : IInvocable
	{
		/// <summary>
		/// Base URL for Disqus API
		/// </summary>
		private const string BASE_URL = "https://disqus.com/api/3.0/";
		/// <summary>
		/// API URL for listing posts
		/// </summary>
		private const string LIST_POSTS_URL = BASE_URL + "categories/listPosts.json";

		private readonly IDisqusCommentRepository _commentRepository;
		private readonly ISiteConfiguration _siteConfiguration;
		private readonly HttpClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="DisqusComments" /> class.
		/// </summary>
		/// <param name="commentRepository">The comment database repository.</param>
		/// <param name="siteConfiguration">The site configuration.</param>
		public DisqusComments(IDisqusCommentRepository commentRepository, ISiteConfiguration siteConfiguration, HttpClient client)
		{
			_commentRepository = commentRepository;
			_siteConfiguration = siteConfiguration;
			_client = client;
		}

		/// <summary>
		/// Synchronise all comments on Disqus into the local database
		/// </summary>
		public async Task Invoke()
		{
			string cursor = string.Empty;
			bool hasMore;

			do
			{
				// TODO: Pass "since" parameter to only get recent comments
				var url = BuildUrl(cursor);
				dynamic data;
				try
				{
					data = await _client.GetDynamicJsonAsync(url);
				}
				catch (HttpRequestException ex)
				{
					throw new Exception("Retrieving Disqus comments failed!", ex);
				}

				foreach (var comment in data.response)
				{
					SyncComment(comment);
				}

				cursor = data.cursor.next;
				hasMore = data.cursor.hasNext;
			} while (hasMore);
		}

		/// <summary>
		/// Synchronise a single comment returned from Disqus
		/// </summary>
		/// <param name="comment"></param>
		private void SyncComment(dynamic comment)
		{
			// Check if this comment already exists
			DisqusCommentModel dbComment = _commentRepository.GetOrDefault((string)comment.id);
			var isNew = dbComment == null;
			if (isNew)
				dbComment = new DisqusCommentModel();

			dbComment.Id = comment.id;
			dbComment.AuthorName = comment.author.name;
			dbComment.AuthorUrl = comment.author.url;
			dbComment.AuthorProfileUrl = comment.author.profileUrl;
			dbComment.AuthorImage = comment.author.avatar.permalink;
			dbComment.Content = comment.message;
			dbComment.Date = DateTime.Parse((string)comment.createdAt);

			// This is an int in the Disqus API (even though comment ID is a string!)
			dbComment.ParentCommentId = comment.parent != null ? comment.parent.ToString() : null;

			// We expect comments to only have one thread identifier
			if (comment.thread.identifiers.Count != 1)
				throw new Exception(string.Format("Expected 1 thread identifier, but thread '{0}' has {1}", dbComment.Id, comment.thread.identifiers.Length));

			dbComment.ThreadId = comment.thread.id;
			dbComment.ThreadLink = comment.thread.link;
			dbComment.ThreadIdentifier = comment.thread.identifiers[0];

			_commentRepository.Save(dbComment, isNew);
		}

		/// <summary>
		/// Builds the URL to use for the API request
		/// </summary>
		/// <param name="cursor">Cursor for pagination (see http://disqus.com/api/docs/cursors/)</param>
		/// <returns>URL for the API</returns>
		private string BuildUrl(string cursor)
		{
			return LIST_POSTS_URL + new QueryBuilder
			{
				{"api_key", _siteConfiguration.DisqusApiKey},
				{"category", _siteConfiguration.DisqusCategory.ToString()},
				{"related", "thread"},
				{"limit", "100"},
				{"order", "asc"},
				{"cursor", cursor},
			};
		}
	}
}
