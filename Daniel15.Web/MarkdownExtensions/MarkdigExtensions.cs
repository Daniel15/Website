using Markdig.Renderers;

namespace Daniel15.Web.MarkdownExtensions
{
	/// <summary>
	/// Extension methods for Markdig
	/// </summary>
	public static class MarkdigExtensions
	{
		/// <summary>
		/// Replaces a default object renderer.
		/// </summary>
		/// <typeparam name="TOriginal">Type of the object renderer to replace</typeparam>
		/// <param name="renderers">Object renderer collection</param>
		/// <param name="replacement">Replacement object renderer</param>
		/// <returns>The object renderer collection</returns>
		public static ObjectRendererCollection Replace<TOriginal>(
			this ObjectRendererCollection renderers,
			IMarkdownObjectRenderer replacement
		) where TOriginal : IMarkdownObjectRenderer
		{
			var index = renderers.FindIndex(renderer => renderer.GetType() == typeof(TOriginal));
			if (index == -1)
			{
				throw new InvalidOperationException($"Could not find original object renderer for {typeof(TOriginal)}");
			}
			renderers.RemoveAt(index);
			renderers.Insert(index, replacement);
			return renderers;
		}
	}
}
