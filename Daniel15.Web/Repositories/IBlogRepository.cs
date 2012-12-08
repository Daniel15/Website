using System.Collections.Generic;
using Daniel15.Web.Models;

namespace Daniel15.Web.Repositories
{
	/// <summary>
	/// Repository for accessing blog posts
	/// </summary>
	public interface IBlogPostRepository : IRepositoryBase<BlogPostModel>
	{
		/// <summary>
		/// Gets the latest blog posts
		/// </summary>
		/// <returns>Latest blog posts</returns>
		List<BlogPostModel> LatestPosts();
	}
}
