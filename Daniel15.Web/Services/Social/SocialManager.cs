using Daniel15.Web.Models.Blog;

namespace Daniel15.Web.Services.Social
{
	/// <summary>
	/// Used to share posts on all available social networks
	/// </summary>
	public class SocialManager : ISocialManager
	{
		private readonly IList<ISocialShare> _socialShares = new List<ISocialShare>();

		/// <summary>
		/// Initializes a new instance of the <see cref="SocialManager" /> class.
		/// </summary>
		public SocialManager(Twitter twitter, Facebook facebook, Linkedin linkedin, Reddit reddit)
		{
			_socialShares.Add(twitter);
			_socialShares.Add(facebook);
			_socialShares.Add(linkedin);
			_socialShares.Add(reddit);
		}

		/// <summary>
		/// Gets the URL to share this post on all available social networks
		/// </summary>
		/// <param name="post">The blog post</param>
		/// <param name="url">Full URL to this post</param>
		/// <param name="shortUrl">Short URL to this post</param>
		/// <returns>Sharing URLs for this post</returns>
		public IEnumerable<KeyValuePair<ISocialNetwork, string>> ShareUrls(PostModel post, string url, string shortUrl)
		{
			foreach (var sharer in _socialShares)
			{
				yield return new KeyValuePair<ISocialNetwork, string>(sharer, sharer.GetShareUrl(post, url, shortUrl));
			}
		}
	}
}
