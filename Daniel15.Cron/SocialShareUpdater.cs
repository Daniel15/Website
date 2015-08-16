using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using Daniel15.BusinessLayer.Services.Social;
using Daniel15.Data.Entities.Blog;
using Daniel15.Data.Repositories;
using Newtonsoft.Json;

namespace Daniel15.Cron
{
	/// <summary>
	/// Handles updating of social media sharing counts
	/// </summary>
	public class SocialShareUpdater
	{
		private readonly ISocialManager _socialManager;
		private readonly IBlogRepository _blogRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="SocialShareUpdater" /> class.
		/// </summary>
		/// <param name="socialManager">The social manager.</param>
		/// <param name="blogRepository">The blog repository.</param>
		public SocialShareUpdater(ISocialManager socialManager, IBlogRepository blogRepository)
		{
			_socialManager = socialManager;
			_blogRepository = blogRepository;
		}

		/// <summary>
		/// Updates the social media sharing counts of all blog posts
		/// </summary>
		public void Run()
		{
			var posts = _blogRepository.LatestPosts(10000);
			foreach (var post in posts)
			{
				UpdatePost(post);
			}
		}

		/// <summary>
		/// Updates the social media sharing counts of the specified post
		/// </summary>
		/// <param name="post">Post to update</param>
		private void UpdatePost(PostModel post)
		{
			// TODO: Move this elsewhere (Business Layer)
			var urls = GetBlogUrl(post);
			var counts = _socialManager.ShareCounts(post, urls.Url, urls.ShortUrl).ToDictionary(x => x.Key.Id, x => x.Value);
			post.ShareCounts = counts;
			_blogRepository.Save(post);

			var total = counts.Sum(x => x.Value);
			Console.WriteLine("Updated '{0}' ({1} shares in total)", post.Title, total);
		}

		/// <summary>
		/// Gets the URL to the specified blog post
		/// </summary>
		/// <param name="post">Blog post to get URL of</param>
		/// <returns>URL of the blog post</returns>
		private UrlResponse GetBlogUrl(PostModel post)
		{
			// TODO: Remove hard-coded URL from here
			var urlApi = $"http://dan.cx/api/posts/{post.Id}/url";
			using (var client = new WebClient())
			{
				var rawResponse = client.DownloadString(urlApi);
				return JsonConvert.DeserializeObject<UrlResponse>(rawResponse);
			}
			
		}

		[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
		[SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
		private class UrlResponse
		{
			public string Url { get; set; }
			public string ShortUrl { get; set; }
		}
	}
}
