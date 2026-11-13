using ColorCode;

namespace Daniel15.Web.MarkdownExtensions;

/// <summary>
/// Hack for ColorCode to render plain text code blocks the same way as supported languages.
/// </summary>
public class PlainTextLanguage : ILanguage
{
	public string FirstLinePattern => string.Empty;
	public string CssClassName => "plain";
	public string Id => "plain";
	public string Name => "Plain text";
	public IList<LanguageRule> Rules { get; } = new List<LanguageRule>
	{
		// ColorCode needs at least one language rule. This rule will never match.
		new(
			@"((?!))",
			new Dictionary<int, string> { [1] = "plain" }
		)
	};

	public bool HasAlias(string lang) =>
		lang is "text" or "plaintext" or "txt";
}
