using System;
using System.Web.Mvc;
using System.Web.Security;
using Daniel15.Web.Areas.Admin.ViewModels.Blog;
using Daniel15.Web.Repositories;
using System.Linq;
using Daniel15.Web.Extensions;

namespace Daniel15.Web.Areas.Admin.Controllers
{
	/// <summary>
	/// Handles administration of the blog
	/// </summary>
	[Authorize]
	public partial class BlogController : Controller
	{
		private readonly IBlogRepository _blogRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="BlogController" /> class.
		/// </summary>
		/// <param name="blogRepository">The blog repository.</param>
		public BlogController(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}

		/// <summary>
		/// Shows the blog administration homepage
		/// </summary>
		/// <returns></returns>
		public virtual ActionResult Index()
		{
			return View(Views.Index, new IndexViewModel
			{
				PublishedPosts = _blogRepository.PublishedCount(),
				UnpublishedPosts = _blogRepository.UnpublishedCount(),
			});
		}

		/// <summary>
		/// Displays a list of posts in the blog
		/// </summary>
		/// <param name="published">Whether to display published posts (true) or unpublished posts (false)</param>
		/// <returns>A list of posts</returns>
		public virtual ActionResult Posts(bool published)
		{
			var posts = _blogRepository.LatestPostsSummary(100000, published);

			return View(Views.Posts, new PostsViewModel
			{
				Posts = posts,
			});
		}

		/// <summary>
		/// Edits the specified blog post.
		/// </summary>
		/// <param name="slug">Slug of the blog post</param>
		/// <returns></returns>
		[HttpGet]
		public virtual ActionResult Edit(string slug)
		{
			// TODO: Handle creation of a new post (null slug)

			var post = _blogRepository.GetBySlug(slug);

			return View(Views.Edit, new EditViewModel
			{
				Post = post,
				Categories = _blogRepository.Categories(),
				Tags = _blogRepository.Tags(),
				PostCategoryIds = _blogRepository.CategoriesForPost(post).Select(cat => cat.Id).ToList(),
				PostTagIds = _blogRepository.TagsForPost(post).Select(tag => tag.Id).ToList()
			});
		}

		/// <summary>
		/// Save changes to a blog post
		/// </summary>
		/// <param name="slug">Slug of the blog post</param>
		/// <param name="viewModel"></param>
		/// <returns></returns>
		[HttpPost]
		public virtual ActionResult Edit(string slug, EditViewModel viewModel)
		{
			// Ensure valid
			if (!ModelState.IsValid)
			{
				// Not valid - Go back to the edit page
				return Edit(slug);
			}

			// Valid, so save
			var post = _blogRepository.GetBySlug(slug);
			UpdateModel(post, "Post", new[] { "Title", "Slug", "Date", "Published", "RawContent", "MainCategoryId" });
			_blogRepository.Save(post);

			// TODO: Save categories
			// TODO: Save tags

			//TempData["topMessage"] = DateTime.Now.ToLongTimeString() + ": Saved changes to " + Server.HtmlEncode(post.Title);
			return Redirect(Url.BlogEdit(post));
		}
	}
}
