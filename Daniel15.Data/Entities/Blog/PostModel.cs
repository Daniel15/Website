using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web;
using Daniel15.Shared.Extensions;
using Newtonsoft.Json;

namespace Daniel15.Data.Entities.Blog
{
	public class PostModel : ISupportsDisqus
	{
		private const string READ_MORE_COMMENT = "<!--more-->";
		private const string READ_MORE_HTML_MARKER = "<span id=\"read-more\"></span>";
		private const string ESCAPED_PRE_ATTRIBUTE = "escaped=\"true\"";

		/// <summary>
		/// Length of the summary of posts (in characters) for the RSS feed
		/// </summary>
		private const int SUMMARY_LENGTH = 330;

		/// <summary>
		/// Represents a pre tag, used to HTML encode contents of pre tags
		/// </summary>
		private static readonly Regex _preTag = new Regex(@"<pre(?<attributes>[^>]+)>(?<content>[\S\s]+?)</pre>", RegexOptions.Compiled);

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

		/// <summary>
		/// UNIX timestamp this blog article was posted at. This is only for backwards compatibility
		/// with the old database - Use <see cref="Date"/> instead.
		/// </summary>
		public long UnixDate { get; set; }

		[Required]
		public DateTime Date
		{
			get { return DateExtensions.FromUnix(UnixDate); }
			set { UnixDate = value.ToUnix(); }
		}

		/// <summary>
		/// The raw content of this blog post, as retrieved from the database
		/// </summary>
		//[AllowHtml]
		[Required]
		public string RawContent { get; set; }

		public int MainCategoryId { get; set; }

		/// <summary>
		/// The main category of this post
		/// </summary>
		public virtual CategoryModel MainCategory { get; set; }
		/// <summary>
		/// Categories this post is contained in
		/// </summary>
		public virtual ICollection<CategoryModel> Categories { get; set; }
		/// <summary>
		/// Tags this post is tagged with
		/// </summary>
		public virtual ICollection<TagModel> Tags { get; set; }

		/// <summary>
		/// Details of how many times this post was shared on social networking sites
		/// </summary>
		public IDictionary<string, int> ShareCounts
		{
			get { return string.IsNullOrEmpty(RawShareCounts) ? null : JsonConvert.DeserializeObject<IDictionary<string, int>>(RawShareCounts); }
			set { RawShareCounts = JsonConvert.SerializeObject(value); }
		}

		/// <summary>
		/// Raw JSON-encoded blob for <see cref="ShareCounts"/>.
		/// </summary>
		public string RawShareCounts { get; set; }

		/// <summary>
		/// Gets the Disqus identifier for this post (currently just the post ID)
		/// </summary>
		public string DisqusIdentifier
		{
			get { return Id.ToString(); }
		}

		/// <summary>
		/// Gets the processed content of this blog post
		/// </summary>
		/// <returns>The blog post content</returns>
		public string Content()
		{
			// Replace "more" comment with marker <span>.
			var content = RawContent.Replace(READ_MORE_COMMENT, READ_MORE_HTML_MARKER);

			// HTML encode anything in <pre> tags
			// TODO: Preprocess this instead of doing it every time? Implement preprocessing once moved to Markdown
			content = _preTag.Replace(content, match =>
			{
				// If "escaped=true" found then just return it as-is
				if (match.Groups["attributes"].Value.Contains(ESCAPED_PRE_ATTRIBUTE))
					return match.Value.Replace(ESCAPED_PRE_ATTRIBUTE, "");

				return "<pre" + match.Groups["attributes"] + ">" + HttpUtility.HtmlEncode(match.Groups["content"]) + "</pre>";
			});

			return content;
		}

		/// <summary>
		/// Gets the introduction to this blog post - That is, the content up to the "read more" section.
		/// </summary>
		/// <returns>Introduction text</returns>
		public string Intro(out bool showMoreLink)
		{
			var content = Content();

			// No "Read more" section, so just return the whole post
			if (!content.Contains(READ_MORE_HTML_MARKER))
			{
				showMoreLink = false;
				return content;
			}

			showMoreLink = true;
			return content.Substring(0, content.IndexOf(READ_MORE_HTML_MARKER));
		}

		/// <summary>
		/// Gets the plain text blog post introduction, for the RSS feed
		/// </summary>
		/// <returns>First section of blog post as plain text</returns>
		public string PlainTextIntro()
		{
			// Remove other HTML tags
			var intro = _htmlTag.Replace(RawContent, string.Empty);

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