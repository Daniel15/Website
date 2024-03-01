using System.Collections.Generic;
using System.Threading.Tasks;
using Daniel15.Web.Models.Blog;

namespace Daniel15.Web.Services.Social
{
	/// <summary>
	/// Used to share posts on all available social networks
	/// </summary>
	public interface ISocialManager
	{
		/// <summary>
		/// Gets the URL to share this post on all available social networks
		/// </summary>
		/// <param name="post">The blog post</param>
		/// <param name="url">Full URL to this post</param>
		/// <param name="shortUrl">Short URL to this post</param>
		/// <returns>Sharing URLs for this post</returns>
		IEnumerable<KeyValuePair<ISocialNetwork, string>> ShareUrls(PostModel post, string url, string shortUrl);
	}
}
