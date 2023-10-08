using System.Collections.Generic;
using Daniel15.Web.Models.Blog;

namespace Daniel15.Web.Repositories
{
	/// <summary>
	/// Repository for accessing microblog (ie. Tumblr) posts
	/// </summary>
	public interface IMicroblogRepository
	{
		/// <summary>
		/// Gets the latest blog posts
		/// </summary>
		/// <param name="count">Number of posts to return</param>
		/// <param name="offset">Post to start at</param>
		/// <returns>Latest blog posts</returns>
		IEnumerable<MicroblogPostModel> LatestPosts(int count = 10, int offset = 0);
	}
}
