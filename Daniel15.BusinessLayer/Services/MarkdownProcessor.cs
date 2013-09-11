using System;
using System.Net;
using System.Text.RegularExpressions;
using MarkdownDeep;

namespace Daniel15.BusinessLayer.Services
{
	/// <summary>
	/// Service that converts Markdown to HTML using MarkdownDeep. Supports some Github Markdown
	/// extensions.
	/// </summary>
	public class MarkdownProcessor : IMarkdownProcessor
	{
		/// <summary>
		/// GitHub code snippet
		/// </summary>
		/// <example>
		/// ```language
		/// ... code here ...
		/// ```
		/// </example>
		private static readonly Regex _codeRegex = new Regex(@"```(?<language>\w+)?(?<content>[\S\s]+?)```", RegexOptions.Compiled);

		/// <summary>
		/// HTML headings
		/// </summary>
		private static readonly Regex _headingRegex = new Regex(@"<h(?<level>[123456])>(?<content>.+)</h\k<level>>", RegexOptions.Compiled);

		/// <summary>
		/// Processor used to process Markdown
		/// TODO: Use an interface to encapsulate this?
		/// </summary>
		private readonly Markdown _processor;

		public MarkdownProcessor()
		{
			_processor = new Markdown
			{
				ExtraMode = true,
				NewWindowForExternalLinks = true
			};
		}

		/// <summary>
		/// Parse Markdown and compile it to HTML
		/// </summary>
		/// <param name="markdown">Markdown to compile</param>
		/// <returns>HTML output</returns>
		public string Parse(string markdown)
		{
			// Pre-process Github-style code blocks
			var output = _codeRegex.Replace(markdown, ProcessCodeBlock);
			// Actually compile the Markdown into HTML
			output = _processor.Transform(output);
			// Move all headings down a level
			// TODO: Remove this, it's only a hack while the site CSS is messed up and lacks <h1> styles :(
			output = _headingRegex.Replace(output, match => 
				string.Format("<h{0}>{1}</h{0}>", 
					Convert.ToInt32(match.Groups["level"].Value) + 1, 
					match.Groups["content"].Value
				)
			);

			return output;
		}

		/// <summary>
		/// Does required pre-processing for a Regex-matched code block.
		/// </summary>
		/// <param name="match">Regex match</param>
		/// <returns>Code block HTML</returns>
		protected virtual string ProcessCodeBlock(Match match)
		{
			return string.Format("<pre class=\"brush: {0}\">{1}</pre>", 
				match.Groups["language"].Value,
				WebUtility.HtmlEncode(match.Groups["content"].Value)
			);
		}
	}
}
