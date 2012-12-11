using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using ServiceStack.DataAnnotations;

namespace Daniel15.Web.Models.Blog
{
	/// <summary>
	/// Represents a blog post
	/// </summary>
	public class PostModel : PostSummaryModel
	{
		private const string READ_MORE_COMMENT = "<!--more-->";
		private const string READ_MORE_HTML_MARKER = "<span id=\"read-more\"></span>";
		private const string ESCAPED_PRE_ATTRIBUTE = "escaped=\"true\"";

		/// <summary>
		/// Represents a pre tag, used to HTML encode contents of pre tags
		/// </summary>
		private static readonly Regex _preTag = new Regex(@"<pre(?<attributes>[^>]+)>(?<content>[\S\s]+?)</pre>", RegexOptions.Compiled);

		/// <summary>
		/// The raw content of this blog post, as retrieved from the database
		/// </summary>
		[Alias("content")]
		public string RawContent { get; set; }

		[Ignore]
		public IList<CategoryModel> Categories { get; set; }

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
	}
}