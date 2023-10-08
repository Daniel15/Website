using Daniel15.Web.Services.Social;

namespace Daniel15.Web.Models.Blog
{
	/// <summary>
	/// Social network sharing details for a blog post
	/// </summary>
	public class PostSocialNetworkModel
	{
		/// <summary>
		/// Details on the social network
		/// </summary>
		public ISocialNetwork SocialNetwork { get; set; }
		/// <summary>
		/// URL to share this post on the specified social network
		/// </summary>
		public string Url { get; set; }
		/// <summary>
		/// Number of times this post has been shared
		/// </summary>
		public int Count { get; set; }
	}
}
