namespace Daniel15.Web.Services
{
	/// <summary>
	/// Service that converts Markdown to HTML
	/// </summary>
	public interface IMarkdownProcessor
	{
		/// <summary>
		/// Parse Markdown and compile it to HTML.
		/// </summary>
		/// <param name="markdown">Markdown to compile</param>
		/// <returns>HTML output</returns>
		string Parse(string markdown);

		/// <summary>
		/// Parse Markdown and compile it to HTML, including front matter.
		/// </summary>
		/// <typeparam name="T">Type of front matter</typeparam>
		/// <param name="markdownWithFrontMatter">Markdown with front matter</param>
		/// <returns>HTML output, and the front matter</returns>
		(string, T) ParseWithFrontMatter<T>(string markdownWithFrontMatter);
	}
}
