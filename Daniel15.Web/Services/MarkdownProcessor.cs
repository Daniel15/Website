using Daniel15.Web.MarkdownExtensions;
using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;
using Markdown.ColorCode;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;

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
		private readonly MarkdownPipeline _pipeline = new MarkdownPipelineBuilder()
			.UseAdvancedExtensions()
			.UseYamlFrontMatter()
			.UseEmojiAndSmiley()
			.UseColorCode(HtmlFormatterType.Css)
			.Use<HeadingHackExtension>()
			.Build();

		/// <summary>
		/// YAML deserializer for front matter.
		/// </summary>
		private readonly IDeserializer _yamlDeserializer = new DeserializerBuilder()
			.WithNamingConvention(CamelCaseNamingConvention.Instance)
			.WithTypeConverter(new DateTimeConverter(DateTimeKind.Local, formats: ["u"]))
			.Build();

		/// <summary>
		/// Parse Markdown and compile it to HTML.
		/// </summary>
		/// <param name="markdown">Markdown to compile</param>
		/// <returns>HTML output</returns>
		public string Parse(string markdown)
		{
			return Markdig.Markdown.ToHtml(markdown, _pipeline);
		}

		/// <summary>
		/// Parse Markdown and compile it to HTML, including front matter.
		/// </summary>
		/// <typeparam name="T">Type of front matter</typeparam>
		/// <param name="markdownWithFrontMatter">Markdown with front matter</param>
		/// <returns>HTML output, and the front matter</returns>
		public (string, T) ParseWithFrontMatter<T>(string markdownWithFrontMatter)
		{
			var document = Markdig.Markdown.Parse(markdownWithFrontMatter, _pipeline);
			var html = document.ToHtml(_pipeline);

			var yamlBlock = document.Descendants<YamlFrontMatterBlock>().First();
			var yamlString = markdownWithFrontMatter
				.Substring(yamlBlock.Span.Start, yamlBlock.Span.End)
				.Trim()
				// Remove "---" from start and end
				.TrimStart('-')
				.TrimEnd('-');
			
			var yaml = _yamlDeserializer.Deserialize<T>(yamlString);
			
			return (html, yaml);

		}
	}
}
