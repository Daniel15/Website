namespace Daniel15.Web.Models
{
	/// <summary>
	/// Represents an entity that supports Disqus comments.
	/// </summary>
	public interface ISupportsDisqus
	{
		/// <summary>
		/// Gets the Disqus identifier for this entity.
		/// </summary>
		string DisqusIdentifier { get; }
	}
}
