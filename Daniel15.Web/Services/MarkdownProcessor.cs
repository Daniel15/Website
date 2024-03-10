using Daniel15.Web.MarkdownExtensions;
using Markdig;
using Markdown.ColorCode;

namespace Daniel15.Web.Services
{
	/// <summary>
	/// Service that converts Markdown to HTML using MarkdownDeep. Supports some Github Markdown
	/// extensions.
	/// </summary>
	public class MarkdownProcessor : IMarkdownProcessor
	{
		/// <summary>
		/// Markdig pipeline for Markdown compilation.
		/// </summary>
		private readonly MarkdownPipeline _pipeline;

		public MarkdownProcessor()
		{
			_pipeline = new MarkdownPipelineBuilder()
				.UseAdvancedExtensions()
				.UseColorCode(HtmlFormatterType.Css)
				.Use<HeadingHackExtension>()
				.Build();
		}

		/// <summary>
		/// Parse Markdown and compile it to HTML
		/// </summary>
		/// <param name="markdown">Markdown to compile</param>
		/// <returns>HTML output</returns>
		public string Parse(string markdown)
		{
			return Markdig.Markdown.ToHtml(markdown, _pipeline);
		}
	}
}
