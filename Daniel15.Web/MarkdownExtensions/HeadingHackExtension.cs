using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.ComponentModel;

namespace Daniel15.Web.MarkdownExtensions
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
				// Clone the inline contents
				var inline = new ContainerInline();
				var child = heading.Inline.FirstChild;
				while (child != null)
				{
					var next = child.NextSibling;
					child.Remove();
					inline.AppendChild(child);
					child = next;
				}

				// Clone the heading block, and increment the level
				var hackHeading = new HeadingBlock(heading.Parser)
				{
					Level = heading.Level + 1,

					Column = heading.Column,
					HeaderChar = heading.HeaderChar,
					IsBreakable = heading.IsBreakable,
					Inline = inline,
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
