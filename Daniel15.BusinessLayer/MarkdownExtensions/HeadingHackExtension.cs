using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Daniel15.BusinessLayer.MarkdownExtensions
{
	/// <summary>
	/// Temporary hack. Moves all headings down a level (h1 to h2, h2 to h3, etc).
	/// TODO: Remove this, it's only a hack while the site CSS is messed up and lacks h1 styles :(
	/// </summary>
	public class HeadingHackExtension : IMarkdownExtension
	{
		public void Setup(MarkdownPipelineBuilder pipeline)
		{
		}

		public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
		{
			renderer.ObjectRenderers.Replace<HeadingRenderer>(new HeadingHackRenderer());
		}

		private class HeadingHackRenderer : HeadingRenderer
		{
			protected override void Write(HtmlRenderer renderer, HeadingBlock heading)
			{
				// Clone the heading block, and increment the level
				var hackHeading = new HeadingBlock(heading.Parser)
				{
					Level = heading.Level + 1,

					Column = heading.Column,
					HeaderChar = heading.HeaderChar,
					Inline = heading.Inline,
					IsBreakable = heading.IsBreakable,
					IsOpen = heading.IsOpen,
					Line = heading.Line,
					Lines = heading.Lines,
					ProcessInlines = heading.ProcessInlines,
					RemoveAfterProcessInlines = heading.RemoveAfterProcessInlines,
					Span = heading.Span,
				};
				hackHeading.SetAttributes(heading.GetAttributes());
				base.Write(renderer, hackHeading);
			}
		}
	}
}
