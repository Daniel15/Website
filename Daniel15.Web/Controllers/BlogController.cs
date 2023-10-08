using Daniel15.Web.Exceptions;
using Daniel15.Web.Services;
using Daniel15.Web.Services.Social;
using Daniel15.Web.Models.Blog;
using Daniel15.Web.Repositories;
using Daniel15.Web.Extensions;
using Daniel15.Web.ViewModels.Blog;
using Microsoft.AspNetCore.Mvc;

namespace Daniel15.Web.Controllers
{
	/// <summary>
	/// Controller for the main blog pages
	/// </summary>
	public partial class BlogController : Controller
	{
		/// <summary>
		/// Number of blog posts to show on each page
		/// </summary>
		private const int ITEMS_PER_PAGE = 10;
		/// <summary>
		/// One hour in seconds.
		/// </summary>
		private const int ONE_HOUR = 3600;

		private readonly IBlogRepository _blogRepository;
		private readonly IDisqusCommentRepository _commentRepository;
		private readonly IUrlShortener _urlShortener;
		private readonly ISocialManager _socialManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="BlogController" /> class.
		/// </summary>
		/// <param name="blogRepository">The blog post repository.</param>
		/// <param name="commentRepository">The Disqus comment repository</param>
		/// <param name="urlShortener">The URL shortener</param>
		/// <param name="socialManager">The social network manager used to get sharing URLs</param>
		public BlogController(IBlogRepository blogRepository, IDisqusCommentRepository commentRepository, IUrlShortener urlShortener, ISocialManager socialManager)
		{
			_blogRepository = blogRepository;
			_commentRepository = commentRepository;
			_urlShortener = urlShortener;
			_socialManager = socialManager;
		}

		/// <summary>
		/// Returns a listing of blog posts
		/// </summary>
		/// <param name="posts">Posts to be displayed</param>
		/// <param name="count">Number of posts being displayed</param>
		/// <param name="pageNum">Page number of the current pageNum</param>
		/// <param name="viewName">Name of the view to render</param>
		/// <param name="viewModel">View model to pass to the view</param>
		/// <returns>Post listing</returns>
		private ActionResult Listing(IEnumerable<PostModel> posts, int count, int pageNum, string viewName = null, ListingViewModel viewModel = null)
		{
			if (viewName == null)
				viewName = "Index";

			if (viewModel == null)
				viewModel = new ListingViewModel();

			var pages = (int)Math.Ceiling((double)count / ITEMS_PER_PAGE);

			if (pageNum > pages)
				return NotFound();

			viewModel.Posts = posts.Select(post => new PostViewModel
			{
				Post = post, 
				ShortUrl = ShortUrl(post),
				SocialNetworks = GetSocialNetworks(post)
			});
			viewModel.TotalCount = count;
			viewModel.Page = pageNum;
			viewModel.TotalPages = pages;
			return View(viewName, viewModel);
		}

		/// <summary>
		/// Index page of the blog
		/// </summary>
		[ResponseCache(Location = ResponseCacheLocation.Any, Duration = ONE_HOUR)]
		[Route("blog", Order = 1, Name = "BlogHome")]
		[Route("blog/page-{pageNum:int}", Order = 2, Name= "BlogHomePage")]
		public virtual ActionResult Index(int pageNum = 1)
		{
			var count = _blogRepository.PublishedCount();
			var posts = _blogRepository.LatestPosts(ITEMS_PER_PAGE, (pageNum - 1) * ITEMS_PER_PAGE);
			return Listing(posts, count, pageNum, "Index");
		}

		/// <summary>
		/// Viewing a category listing
		/// </summary>
		/// <param name="slug">Category slug</param>
		/// <param name="pageNum">Page number to view</param>
		/// <param name="parentSlug">Slug of the category's parent</param>
		/// <returns>Posts in this category</returns>
		/// <remarks>These must be ordered AFTER the RSS rules in FeedController!</remarks>
		[Route("category/{slug}", Order = 5, Name = "BlogCategory")]
		[Route("category/{slug}/page-{pageNum:int}", Order = 6, Name = "BlogCategoryPage")]
		[Route("category/{parentSlug}/{slug}", Order = 7, Name = "BlogSubCategory")]
		[Route("category/{parentSlug}/{slug}/page-{pageNum:int}", Order = 8, Name = "BlogSubCategoryPage")]
		public virtual ActionResult Category(string slug, int pageNum = 1, string parentSlug = null)
		{
			CategoryModel category;
			try
			{
				category = _blogRepository.GetCategory(slug);
			}
			catch (EntityNotFoundException)
			{
				// Throw a 404 if the category doesn't exist
				return NotFound();
			}

			// If the category has a parent category, ensure it's in the URL
			if (category.Parent != null && string.IsNullOrEmpty(parentSlug))
			{
				return RedirectPermanent(Url.BlogCategory(category, pageNum));
			}

			var count = _blogRepository.PublishedCount(category);
			var posts = _blogRepository.LatestPosts(category, ITEMS_PER_PAGE, (pageNum - 1) * ITEMS_PER_PAGE);
			return Listing(posts, count, pageNum, "Category", new CategoryListingViewModel
			{
				Category = category,
				RssUrl = Url.Action("BlogCategory", "Feed", new
				{
					Slug = category.Slug,
					ParentSlug = category.Parent?.Slug
				}, Request.Scheme)
			});
		}

		/// <summary>
		/// Viewing a listing of all posts tagged by a particular tag
		/// </summary>
		/// <param name="slug">Tag slug</param>
		/// <param name="pageNum">Page number to view</param>
		/// <returns>Posts tagged with this tag</returns>
		[Route("tag/{slug}", Order = 1, Name = "BlogTag")]
		[Route("tag/{slug}/page-{pageNum:int}", Order = 2, Name = "BlogTagPage")]
		public virtual ActionResult Tag(string slug, int pageNum = 1)
		{
			TagModel tag;
			try
			{
				tag = _blogRepository.GetTag(slug);
			}
			catch (EntityNotFoundException)
			{
				// Throw a 404 if the category doesn't exist
				return NotFound();
			}

			var count = _blogRepository.PublishedCount(tag);
			var posts = _blogRepository.LatestPosts(tag, ITEMS_PER_PAGE, (pageNum - 1) * ITEMS_PER_PAGE);
			return Listing(posts, count, pageNum, "Tag", new TagListingViewModel { Tag = tag });
		}

		/// <summary>
		/// Viewing the blog archive (articles posted in the specified year and month)
		/// </summary>
		/// <param name="year">Year to get posts for</param>
		/// <param name="month">Month to get posts for</param>
		/// <param name="pageNum">Page number to view</param>
		/// <returns>Posts from this month</returns>
		[Route("{year:int:length(4)}/{month:int:length(2)}")]
		public virtual ActionResult Archive(int year, int month, int pageNum = 1)
		{
			var count = _blogRepository.PublishedCountForMonth(year, month);
			var posts = _blogRepository.LatestPostsForMonth(year, month, ITEMS_PER_PAGE, (pageNum - 1) * ITEMS_PER_PAGE);
			return Listing(posts, count, pageNum, "Index");
		}

		/// <summary>
		/// Viewing a blog post
		/// </summary>
		/// <param name="month">The month of the post</param>
		/// <param name="year">The year of the post</param>
		/// <param name="slug">The slug.</param>
		/// <returns>Blog post page</returns>
		[Route("{year:int:length(4)}/{month:int:length(2)}/{slug}")]
		public virtual ActionResult View(int month, int year, string slug)
		{
			PostModel post;
			try
			{
				post = _blogRepository.GetBySlug(slug);
			}
			catch (EntityNotFoundException)
			{
				// Throw a 404 if the post doesn't exist
				return NotFound();
			}

			// Check the URL was actually correct (year and month), redirect if not.
			if (year != post.Date.Year || month != post.Date.Month)
			{
				return RedirectPermanent(Url.BlogPost(post));
			}

			// Set last-modified date based on the date of the post
			Response.Headers["Last-Modified"] = post.Date.ToUniversalTime().ToString("R");

			return View(new PostViewModel
			{
				Post = post,
				PostCategories = _blogRepository.CategoriesForPost(post),
				PostTags = _blogRepository.TagsForPost(post),
				ShortUrl = Url.Action("Blog", "ShortUrl", new { Alias = _urlShortener.Shorten(post) }, Request.Scheme),
				SocialNetworks = GetSocialNetworks(post),
				Comments = _commentRepository.GetCommentsTree(post)
			});
		}


		/// <summary>
		/// Gets the short URL for this blog post
		/// </summary>
		/// <param name="post">Blog post</param>
		/// <returns>The short URL</returns>
		private string ShortUrl(PostModel post)
		{
			return Url.Action("Blog", "ShortUrl", new { Alias = _urlShortener.Shorten(post) }, Request.Scheme);
		}

		/// <summary>
		/// Gets the social network URLs and share counts for the specified post
		/// </summary>
		/// <param name="post">Post to get statistics on</param>
		/// <returns>Social network URLs and share counts for the post</returns>
		private IEnumerable<PostSocialNetworkModel> GetSocialNetworks(PostModel post)
		{
			var shareCounts = post.ShareCounts ?? new Dictionary<string, int>();
			var socialNetworks = _socialManager.ShareUrls(post, Url.BlogPostAbsolute(post), ShortUrl(post));

			return socialNetworks.Select(x => new PostSocialNetworkModel
			{
				SocialNetwork = x.Key,
				Url = x.Value,
				Count = shareCounts.ContainsKey(x.Key.Id) ? shareCounts[x.Key.Id] : 0
			});
		}
	}
}
