using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Daniel15.Web.Models.Blog
{
	public class PostModel : ISupportsDisqus
	{
		public const string READ_MORE_COMMENT = "<!--more-->";
		public const string READ_MORE_HTML_MARKER = "<span id=\"read-more\"></span>";
		public const string POST_ATTRIBUTION_PREFIX = "This post is originally from Daniel15's Blog at ";

		/// <summary>
		/// Length of the summary of posts (in characters) for the RSS feed
		/// </summary>
		private const int SUMMARY_LENGTH = 330;

		/// <summary>
		/// Matches any HTML tag
		/// </summary>
		private static readonly Regex _htmlTag = new Regex(@"<[^>]+>", RegexOptions.Compiled);

		public int Id { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Slug { get; set; }
		public bool Published { get; set; }
		public string Summary { get; set; }

		[Required]
		public DateTime Date { get; set; }

		/// <summary>
		/// The raw content of this blog post, as retrieved from the database
		/// </summary>
		//[AllowHtml]
		[Required]
		public string Content { get; set; }

		public int MainCategoryId { get; set; }

		/// <summary>
		/// The main category of this post
		/// </summary>
		public virtual CategoryModel MainCategory { get; set; }
		/// <summary>
		/// Categories this post is contained in
		/// </summary>
		public virtual List<PostCategoryModel> PostCategories { get; set; }
		/// <summary>
		/// Tags this post is tagged with
		/// </summary>
		public virtual List<PostTagModel> PostTags { get; set; }

		/// <summary>
		/// Details of how many times this post was shared on social networking sites
		/// </summary>
		public IDictionary<string, int> ShareCounts { get; set; }

		/// <summary>
		/// Gets the Disqus identifier for this post (currently just the post ID)
		/// </summary>
		public string DisqusIdentifier => Id.ToString();

		/// <summary>
		/// Gets the introduction to this blog post - That is, the content up to the "read more" section.
		/// </summary>
		/// <returns>Introduction text</returns>
		public string Intro(out bool showMoreLink)
		{
			var content = Content;

			// No "Read more" section, so just return the whole post
			if (!content.Contains(READ_MORE_HTML_MARKER))
			{
				showMoreLink = false;
				return content;
			}

			showMoreLink = true;
			return content.Substring(0, content.IndexOf(READ_MORE_HTML_MARKER, StringComparison.Ordinal));
		}

		/// <summary>
		/// Gets the plain text blog post introduction, for the RSS feed
		/// </summary>
		/// <returns>First section of blog post as plain text</returns>
		public string PlainTextIntro()
		{
			// Remove other HTML tags
			var intro = _htmlTag.Replace(Content, string.Empty);

			// Now trim it if needed
			if (intro.Length > SUMMARY_LENGTH)
			{
				intro = intro.Substring(0, SUMMARY_LENGTH) + "...";
			}

			return intro;
		}

		/// <summary>
		/// Gets the summary text if available, otherwise passes through to <see cref="PlainTextIntro"/>
		/// to get a plain text introduction.
		/// </summary>
		public string SummaryOrPlainTextIntro()
		{
			return string.IsNullOrWhiteSpace(Summary) ? PlainTextIntro() : Summary;
		}
	}
}
