using System;
using System.Web.Mvc;
using System.Web.Security;
using Daniel15.Web.Areas.Admin.ViewModels.Blog;
using Daniel15.Web.Models.Blog;
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
		/// <param name="tempDataProvider">The temporary data provider</param>
		public BlogController(IBlogRepository blogRepository, ITempDataProvider tempDataProvider)
		{
			_blogRepository = blogRepository;
			// TODO: This shouldn't be required to be passed in the constructor - Can set it as a property.
			TempDataProvider = tempDataProvider;
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
		public virtual ActionResult Edit(string slug = null)
		{
			// If slug is not specified, we're creating a new post
			var post = string.IsNullOrEmpty(slug)
				? new PostModel { Date = DateTime.Now }
				: _blogRepository.GetBySlug(slug);

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
		public virtual ActionResult Edit(EditViewModel viewModel, string slug = null)
		{
			// Ensure valid
			if (!ModelState.IsValid)
			{
				// Not valid - Go back to the edit page
				return Edit(slug);
			}

			// If slug is not specified, we're creating a new post
			var post = string.IsNullOrEmpty(slug)
				? new PostModel()
				: _blogRepository.GetBySlug(slug);

			// Valid, so save the post using a whitelist of fields allowed to be updated from the UI
			UpdateModel(post, "Post", new[] { "Title", "Slug", "Date", "Published", "RawContent", "MainCategoryId" }); 
			_blogRepository.Save(post);

			// Now save categories and tags
			// Make sure main category is always included in categories
			var categories = (viewModel.PostCategoryIds ?? Enumerable.Empty<int>()).Union(new[] { viewModel.Post.MainCategoryId });

			_blogRepository.SetCategories(post, categories);
			_blogRepository.SetTags(post, viewModel.PostTagIds ?? Enumerable.Empty<int>());

			TempData["topMessage"] = string.Format(
				"{0}: Saved changes to {1}. <a href=\"{2}\" target=\"_blank\">View post</a>.", DateTime.Now.ToLongTimeString(),
				Server.HtmlEncode(post.Title), Url.Blog(post));
			return Redirect(Url.BlogEdit(post));
		}
	}
}
