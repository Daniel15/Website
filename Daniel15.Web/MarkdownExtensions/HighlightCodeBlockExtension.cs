using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Daniel15.Web.MarkdownExtensions
{
	/// <summary>
	/// Replaces Markdig's built-in code block renderer with one that renders HTML suitable for
	/// Highlight.js.
	/// </summary>
	public class HighlightCodeBlockExtension : IMarkdownExtension
	{
		public void Setup(MarkdownPipelineBuilder pipeline)
		{
		}

		public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
		{
			renderer.ObjectRenderers.Replace<CodeBlockRenderer>(new HighlightCodeBlockRenderer());
		}

		private class HighlightCodeBlockRenderer : HtmlObjectRenderer<CodeBlock>
		{
			protected override void Write(HtmlRenderer renderer, CodeBlock codeBlock)
			{
				var languageAttribute = string.Empty;
				var fencedCodeBlock = codeBlock as FencedCodeBlock;
				if (fencedCodeBlock != null)
				{
					languageAttribute = $" class=\"brush: {fencedCodeBlock.Info}\"";
				}


				renderer.Write("<pre");
				renderer.Write(languageAttribute);
				renderer.Write(">");
				renderer.WriteLeafRawLines(codeBlock, writeEndOfLines: true, escape: true);
				renderer.WriteLine("</pre>");
			}
		}
	}
}
