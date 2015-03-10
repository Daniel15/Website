using System;
using System.Collections.Generic;

namespace Daniel15.Data.Entities.Blog
{
	/// <summary>
	/// Represents a comment imported from Disqus.
	/// </summary>
	public class DisqusCommentModel
	{
		public string Id { get; set; }

		public string ThreadId { get; set; }

		public string ThreadLink { get; set; }

		public string ThreadIdentifier { get; set; }

		public string ParentCommentId { get; set; }

		public string Content { get; set; }

		public string AuthorName { get; set; }

		public string AuthorUrl { get; set; }

		public string AuthorProfileUrl { get; set; }

		public string AuthorImage { get; set; }

		public DateTime Date { get; set; }

		public IList<DisqusCommentModel> Children { get; set; }
	}
}
