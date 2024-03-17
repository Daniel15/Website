using System.Text;
using Coravel.Invocable;
using Daniel15.Web.Exceptions;
using Daniel15.Web.Models.Blog;
using Daniel15.Web.Repositories;

namespace Daniel15.Web.Services;

public class BlogMarkdownImporter(
	IHostEnvironment env,
	ILogger<BlogMarkdownImporter> logger,
	IBlogRepository blogRepository,
	IMarkdownProcessor markdown
) : IInvocable
{
	public async Task Invoke()
	{
		logger.LogInformation("Starting Markdown importer.");
		var postsDir = Path.Combine(env.ContentRootPath, "Posts");

		// It's fine to do these in series instead of parallel... This only happens on app
		// startup and doesn't take long.
		foreach (var file in Directory.GetFiles(postsDir, "*.md"))
		{
			await ProcessFile(file);
		}

	}

	private async Task ProcessFile(string path)
	{
		var filename = Path.GetFileNameWithoutExtension(path);
		var slug = filename.Split('-', 4)[3];
		logger.LogInformation($"Importing {slug}");

		// Load Markdown
		var markdownWithFrontMatter = await File.ReadAllTextAsync(path, Encoding.UTF8);
		var (content, metadata) = 
			markdown.ParseWithFrontMatter<PostFrontMatter>(markdownWithFrontMatter);

		// Load existing post (or create new one)
		PostModel post;
		try
		{
			post = blogRepository.GetBySlug(slug);
		}
		catch (EntityNotFoundException)
		{
			logger.LogInformation("*** This is a new post!");
			post = new PostModel();
		}

		post.Date = metadata.PublishedDate;
		post.Id = metadata.Id;
		post.Slug = slug;
		post.Summary = metadata.Summary;
		post.Title = metadata.Title;

		var categories = blogRepository.GetCategoriesByTitle(metadata.Categories);
		post.MainCategory = categories[0];
		post.PostCategories = categories.Select(category => new PostCategoryModel { Category = category, Post = post}).ToList();

		// Replace "more" comment with marker <span>.
		// TODO: The Markdown parser should probably do this.
		content = content.Replace(PostModel.READ_MORE_COMMENT, PostModel.READ_MORE_HTML_MARKER);
		post.Content = content;

		blogRepository.Save(post);
		logger.LogInformation($"Saved {slug}");
	}
}
