using Daniel15.Web.Models.Blog;

namespace Daniel15.Web.ViewModels.Blog
{
	/// <summary>
	/// Represents a post and some additional data related to it.
	/// </summary>
	public class PostViewModel : ViewModelBase
	{
		/// <summary>
		/// The actual post.
		/// </summary>
		public PostModel Post { get; set; }

		/// <summary>
		/// Short URL to the post, for sharing.
		/// </summary>
		public string ShortUrl { get; set; }

		/// <summary>
		/// List of all the categories this post is included in.
		/// </summary>
		public IEnumerable<CategoryModel> PostCategories { get; set; }

		/// <summary>
		/// List of all tags this post is tagged with.
		/// </summary>
		public IList<TagModel> PostTags { get; set; }

		/// <summary>
		/// All the cached Disqus comments for this post
		/// </summary>
		public IEnumerable<DisqusCommentModel> Comments { get; set; }

		/// <summary>
		/// All the social network sharing URLs for this post
		/// </summary>
		public IEnumerable<PostSocialNetworkModel> SocialNetworks { get; set; }
	}
}
