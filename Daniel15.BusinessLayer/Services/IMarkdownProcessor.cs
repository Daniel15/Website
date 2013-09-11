namespace Daniel15.BusinessLayer.Services
{
	/// <summary>
	/// Service that converts Markdown to HTML
	/// </summary>
	public interface IMarkdownProcessor
	{
		/// <summary>
		/// Parse Markdown and compile it to HTML
		/// </summary>
		/// <param name="markdown">Markdown to compile</param>
		/// <returns>HTML output</returns>
		string Parse(string markdown);
	}
}
