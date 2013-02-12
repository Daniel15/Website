using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Daniel15.Shared.Extensions;
using ServiceStack.DataAnnotations;

namespace Daniel15.Data.Entities.Blog
{
	[Alias("blog_posts")]
	public class PostSummaryModel : ISupportsDisqus
	{
		public int Id { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Slug { get; set; }
		public bool Published { get; set; }

		/// <summary>
		/// UNIX timestamp this blog article was posted at. This is only for backwards compatibility
		/// with the old database - Use <see cref="Date"/> instead.
		/// </summary>
		[Alias("date")]
		public long UnixDate { get; set; }

		[Ignore]
		[Required]
		public DateTime Date
		{
			get { return DateExtensions.FromUnix(UnixDate); }
			set { UnixDate = value.ToUnix(); }
		}

		/// <summary>
		/// Gets the Disqus identifier for this post (currently just the post ID)
		/// </summary>
		[Ignore]
		public string DisqusIdentifier
		{
			get { return Id.ToString(); }
		}
	}
}