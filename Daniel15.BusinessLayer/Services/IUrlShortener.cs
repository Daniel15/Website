using Daniel15.Data.Entities.Blog;

namespace Daniel15.BusinessLayer.Services
{
	public interface IUrlShortener
	{
		/// <summary>
		/// Gets the short URL for this blog post
		/// </summary>
		/// <param name="post">Blog post</param>
		/// <returns>The short URL</returns>
		string Shorten(PostModel post);

		/// <summary>
		/// Convert a short URL back to an ID
		/// </summary>
		/// <param name="alias">The short URL</param>
		/// <returns>ID represented by this short URL</returns>
		int Extend(string alias);
	}
}
