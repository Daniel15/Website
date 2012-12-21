using System;
using System.ComponentModel.DataAnnotations;
using Daniel15.Web.Extensions;
using ServiceStack.DataAnnotations;

namespace Daniel15.Web.Models.Blog
{
	[Alias("blog_posts")]
	public class PostSummaryModel
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
	}
}