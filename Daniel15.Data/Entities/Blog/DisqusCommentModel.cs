using System;
using ServiceStack.DataAnnotations;

namespace Daniel15.Data.Entities.Blog
{
	/// <summary>
	/// Represents a comment imported from Disqus.
	/// </summary>
	[Alias("disqus_comments")]
	public class DisqusCommentModel
	{
		[Alias("id")]
		public string Id { get; set; }

		[Alias("thread_id")]
		public string ThreadId { get; set; }

		[Alias("thread_link")]
		public string ThreadLink { get; set; }

		[Alias("thread_identifier")]
		public string ThreadIdentifier { get; set; }

		[Alias("parent_comment_id")]
		public string ParentCommentId { get; set; }

		[Alias("content")]
		public string Content { get; set; }

		[Alias("author_name")]
		public string AuthorName { get; set; }

		[Alias("author_url")]
		public string AuthorUrl { get; set; }

		[Alias("author_profile_url")]
		public string AuthorProfileUrl { get; set; }

		[Alias("author_image")]
		public string AuthorImage { get; set; }

		[Alias("date")]
		public DateTime Date { get; set; }
	}
}
