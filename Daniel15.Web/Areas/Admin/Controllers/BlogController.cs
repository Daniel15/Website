using System;
using Daniel15.BusinessLayer.Services;
using Daniel15.Data.Entities.Blog;
using Daniel15.Data.Repositories;
using Daniel15.Web.Areas.Admin.ViewModels.Blog;
using System.Linq;
using Daniel15.Web.Extensions;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.WebEncoders;

namespace Daniel15.Web.Areas.Admin.Controllers
{
	/// <summary>
	/// Handles administration of the blog
	/// </summary>
	[Authorize]
	[Area("Admin")]
	[Route("blog/admin")]
	public partial class BlogController : Controller
	{
		private readonly IBlogRepository _blogRepository;
		private readonly IDisqusComments _comments;

		/// <summary>
		/// Initializes a new instance of the <see cref="BlogController" /> class.
		/// </summary>
		/// <param name="blogRepository">The blog repository.</param>
		/// <param name="comments">Disqus comments service</param>
		public BlogController(IBlogRepository blogRepository, IDisqusComments comments)
		{
			_blogRepository = blogRepository;
			_comments = comments;
		}

		/// <summary>
		/// Shows the blog administration homepage
		/// </summary>
		/// <returns></returns>
		[Route("")]
		public virtual ActionResult Index()
		{
			return View("Index", new IndexViewModel
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
		[Route("posts")]
		public virtual ActionResult Posts(bool published)
		{
			var posts = _blogRepository.LatestPosts(count: 100000, published: published);

			return View("Posts", new PostsViewModel
			{
				Posts = posts,
			});
		}

		/// <summary>
		/// Edits the specified blog post.
		/// </summary>
		/// <param name="slug">Slug of the blog post</param>
		/// <returns></returns>
		[HttpGet("~/{year:int:length(4)}/{month:int:length(2)}/{slug}/edit", Order = 1)]
		[HttpGet("~/blog/admin/new", Order = 2)]
		public virtual ActionResult Edit(string slug = null)
		{
			// If slug is not specified, we're creating a new post
			var post = string.IsNullOrEmpty(slug)
				? new PostModel { Date = DateTime.Now }
				: _blogRepository.GetBySlug(slug);

			return View("Edit", new EditViewModel
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
		[HttpPost("~/{year:int:length(4)}/{month:int:length(2)}/{slug}/edit", Order = 1)]
		[HttpPost("~/blog/admin/new", Order = 2)]
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
			// Currently broken due to https://github.com/aspnet/Mvc/issues/2799, so for now we'll just
			// manually update all the things.
			//await TryUpdateModelAsync(post, "Post", x => x.Title, x => x.Slug, x => x.Date, x => x.Published, x => x.RawContent, x => x.MainCategoryId, x => x.Summary);
			post.Title = viewModel.Post.Title;
			post.Slug = viewModel.Post.Slug;
			post.Date = viewModel.Post.Date;
			post.Published = viewModel.Post.Published;
			post.RawContent = viewModel.Post.RawContent;
			post.MainCategoryId = viewModel.Post.MainCategoryId;
			post.Summary = viewModel.Post.Summary;

			_blogRepository.Save(post);

			// Now save categories and tags
			// Make sure main category is always included in categories
			var categories = (viewModel.PostCategoryIds ?? Enumerable.Empty<int>()).Union(new[] { viewModel.Post.MainCategoryId });

			_blogRepository.SetCategories(post, categories);
			_blogRepository.SetTags(post, viewModel.PostTagIds ?? Enumerable.Empty<int>());

			TempData["topMessage"] = string.Format(
				"{0}: Saved changes to {1}. <a href=\"{2}\" target=\"_blank\">View post</a>.",
				DateTime.Now.ToLongTimeString(),
				HtmlEncoder.Default.HtmlEncode(post.Title), Url.BlogPost(post)
			);
			return Redirect(Url.BlogPostEdit(post));
		}

		[Route("synccomments")]
		public virtual ActionResult SyncComments()
		{
			_comments.Sync();
			TempData["topMessage"] = "All done!";
			return RedirectToAction("Index");
		}
	}
}
