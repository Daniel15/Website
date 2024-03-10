using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace Daniel15.MigrateBlogPostsToMarkdown;

internal static class HtmlToMarkdown
{
	private const string BRUSH_PREFIX = "brush: ";
	private const string ESCAPED_PRE_ATTRIBUTE = "escaped=\"true\"";

	// Copied from PostModel.cs
	/// <summary>
	/// Represents a pre tag, used to HTML encode contents of pre tags
	/// </summary>
	private static readonly Regex _preTag = new(@"<pre(?<attributes>[^>]+)>(?<content>[\S\s]+?)</pre>", RegexOptions.Compiled);

	public static async Task<string> ConvertAsync(string html)
	{
		// HTML encode anything in <pre> tags, as this was not encoded before
		// Copied from PostModel.cs
		html = _preTag.Replace(html, match =>
		{
			// If "escaped=true" found then just return it as-is
			if (match.Groups["attributes"].Value.Contains(ESCAPED_PRE_ATTRIBUTE))
				return match.Value.Replace(ESCAPED_PRE_ATTRIBUTE, "");

			return "<pre" + match.Groups["attributes"] + ">" + HtmlEncoder.Default.Encode(match.Groups["content"].Value) + "</pre>";
		})
			.ReplaceLineEndings();

		var config = Configuration.Default;
		var context = BrowsingContext.New(config);

		var doc = await context.OpenAsync(x => x.Content(html));

		var builder = new StringBuilder();
		// footerBuilder is for anything that needs to go below the text, for example abbreviations.
		var footerBuilder = new StringBuilder();
		RecursivelyVisitChildren(doc.Body, builder, footerBuilder);

		if (footerBuilder.Length > 0)
		{
			builder.AppendLine();
			builder.Append(footerBuilder);
		}

		var result = builder.ToString().ReplaceLineEndings();
		// Remove excess line breaks
		return result.Replace("\r\n\r\n\r\n", "\r\n\r\n");
	}

	private static void RecursivelyVisitChildren(
		INode element,
		StringBuilder builder,
		StringBuilder footerBuilder
	)
	{
		foreach (var child in element.ChildNodes)
		{
			Visit(child, builder, footerBuilder);
		}
	}

	private static void Visit(
		INode element,
		StringBuilder builder,
		StringBuilder footerBuilder
	)
	{
		switch (element)
		{
			case IText:
				builder.Append(element.NodeValue);
				break;

			case IComment:
				// Keep comments as-is
				builder.AppendLine(element.ToHtml());
				break;

			case IHtmlElement htmlElement:
				VisitHtmlElement(htmlElement, builder, footerBuilder);
				break;
		}
	}
	private static void VisitHtmlElement(
		IHtmlElement element,
		StringBuilder builder,
		StringBuilder footerBuilder
	)
	{
		switch (element.TagName.ToLowerInvariant())
		{
			case "a":
				if (element.TextContent == element.GetAttribute("href"))
				{
					// <a href="https://example.com">example.com</a> ==> https://example.com
					builder.Append(element.TextContent);
				}
				else
				{
					// <a href="https://example.com">Cool Link</a> ==> [Cool Link](https://example.com)
					builder.Append('[');
					Recurse();
					builder.Append("](");
					builder.Append(element.GetAttribute("href"));
					builder.Append(')');
					builder.Append(FormatAttributes(element, new HashSet<string> {"href", "target"}));
				}
				break;


			case "acronym":
				var full = element.GetAttribute("title");
				var shortened = element.TextContent;

				// Ref: https://github.com/xoofx/markdig/blob/master/src/Markdig.Tests/Specs/AbbreviationSpecs.md
				footerBuilder.Append("*[");
				footerBuilder.Append(shortened);
				footerBuilder.Append("]: ");
				footerBuilder.AppendLine(full);

				builder.Append(shortened);
				break;

			case "blockquote":
				var childBuilder = new StringBuilder();
				RecursivelyVisitChildren(element, childBuilder, footerBuilder);
				var childText = string.Join(
					'\n',
					childBuilder.ToString()
						.Trim()
						.Split('\n')
						.Select(line => $"> {line}")
				);
				builder.AppendLine(childText);
				break;

			case "br":
				builder.Append("  \r\n");
				break;

			case "em":
			case "i":
				builder.Append('*');
				Recurse();
				builder.Append('*');
				break;

			// Hax: Blog posts only used H2 and H3, which was only due to issues with the CSS
			// on the site. Move them down a level (h3 -> h2 and h2 -> h1)
			case "h2":
				builder.Append("# ");
				Recurse();
				break;

			case "h3":
				builder.Append("## ");
				Recurse();
				break;

			case "h4":
				builder.Append("### ");
				Recurse();
				break;

			case "hr":
				builder.AppendLine();
				builder.AppendLine("---");
				break;

			case "img":
				builder.Append("![");
				var alt = element.GetAttribute("alt");
				if (alt != null)
				{
					builder.Append(alt);
				}

				builder.Append("](");
				builder.Append(element.GetAttribute("src"));
				builder.Append(')');
				builder.Append(FormatAttributes(element, new HashSet<string> {"alt", "border", "id", "src"}));
				break;

			case "object":
				// See if it's an old YouTube embed (Flash version)
				// Ref: https://github.com/xoofx/markdig/blob/master/src/Markdig.Tests/Specs/MediaSpecs.md
				var param = element.QuerySelector("param[name=movie]");
				if (param != null)
				{
					builder.Append("![youtube.com](");
					builder.Append(param.GetAttribute("value"));
					builder.Append(')');
					return;
				}

				throw new Exception("Unrecognized <object>");

			case "ol":
			{
				var items = element.GetElementsByTagName("li");
				var i = 1;
				foreach (var item in items)
				{
					builder.Append(i);
					builder.Append(". ");
					RecursivelyVisitChildren(item, builder, footerBuilder);
					builder.AppendLine();
					++i;
				}

				break;
			}

			case "p":
				Recurse();
				builder.AppendLine();
				break;

			case "pre":
				builder.Append("```");
				var cssClass = element.GetAttribute("class");
				if (cssClass?.StartsWith(BRUSH_PREFIX) ?? false)
				{
					builder.Append(cssClass.Replace(BRUSH_PREFIX, string.Empty));
				}
				builder.AppendLine();

				builder.AppendLine(element.TextContent.Trim());
				builder.Append("```");
				break;

			case "script":
				// No-op... Some individual posts had Digg buttons embedded.
				break;

			case "span":
				Recurse();
				builder.Append(FormatAttributes(element, new HashSet<string>()));
				break;

			case "strong":
			case "b":
				builder.Append("**");
				Recurse();
				builder.Append("**");
				break;

			case "tt":
			case "code":
				builder.Append('`');
				Recurse();
				builder.Append('`');
				break;

			case "ul":
			{
				var items = element.GetElementsByTagName("li");
				foreach (var item in items)
				{
					builder.Append("* ");
					RecursivelyVisitChildren(item, builder, footerBuilder);
					builder.AppendLine();
				}

				break;
			}

			default:
				throw new Exception($"Unknown tag {element.TagName}");
		}

		return;
		void Recurse()
		{
			RecursivelyVisitChildren(element, builder, footerBuilder);
		}
	}

	// Ref: https://github.com/xoofx/markdig/blob/master/src/Markdig.Tests/Specs/GenericAttributesSpecs.md
	private static string FormatAttributes(IHtmlElement element, IReadOnlySet<string> exclude)
	{
		var attributes = new List<string>();
		foreach (var attribute in element.Attributes)
		{
			if (exclude.Contains(attribute.Name))
			{
				continue;
			}

			var attrValue = attribute.Value;
			if (attrValue.Contains(' '))
			{
				attrValue = '"' + attrValue + '"';
			}

			attributes.Add(attribute.Name + "=" + attrValue);
		}

		return attributes.Count == 0 ? "" : "{" + string.Join(' ', attributes) + "}";
	}
}
