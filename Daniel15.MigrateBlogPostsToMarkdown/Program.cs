using System.Text.RegularExpressions;
using Daniel15.Web;
using Daniel15.Web.Models.Blog;
using Daniel15.Web.Repositories;
using Daniel15.Web.Repositories.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;

namespace Daniel15.MigrateBlogPostsToMarkdown;

internal class Program(IBlogRepository blogRepository)
{
	private const string BASE_DIR = @"C:\src\dan.cx\Daniel15.Web\Posts";
	private readonly Regex _blankSummaryRegex = new("summary: \r\n", RegexOptions.Compiled);

	private readonly ISerializer _yamlSerializer = new SerializerBuilder()
		.WithNamingConvention(CamelCaseNamingConvention.Instance)
		.WithTypeConverter(new DateTimeConverter(DateTimeKind.Local, formats: ["u"]))
		.Build();

	public static async Task Main(string[] args)
	{
		var builder = Host.CreateApplicationBuilder(args);

		var services = builder.Services;
		var config = builder.Configuration;

		config
			.AddJsonFile("config.json", optional: false)
			.AddJsonFile($"config.Development.json", optional: false);

		services.AddTransient<Program>();

		// Database
		services.AddDbContext<DatabaseContext>(options =>
			options.UseMySql(
				config["Data:DefaultConnection:ConnectionString"],
				ServerVersion.AutoDetect(config["Data:DefaultConnection:ConnectionString"])
			)
		);
		services.AddScoped<IBlogRepository, BlogRepository>();

		using var host = builder.Build();
		await host.Services.GetRequiredService<Program>().Run();
	}

	public async Task Run()
	{
		Directory.CreateDirectory(BASE_DIR);

		// Test with just one post
		// await RunForPostAsync(blogRepository.GetBySlug("restore-mysql-dump-using-php"));

		var allPosts = blogRepository.All();
		Console.WriteLine($"{allPosts.Count} blog posts found.");
		foreach (var post in allPosts)
		{
			await RunForPostAsync(post);
		}
	}

	public async Task RunForPostAsync(PostModel postWithoutSomeData)
	{
		// .GetBySlug() gets some fields that .All() doesn't... :/
		var post = blogRepository.GetBySlug(postWithoutSomeData.Slug);
		Console.WriteLine($"Processing {post.Title} ({post.Id})");

		var contents = $"""
		                ---
		                {BuildFrontMatter(post)}
		                ---

		                {await HtmlToMarkdown.ConvertAsync(post.RawContent)}
		                """;

		var filename = $"{post.Date:yyyy-MM-dd}-{post.Slug}.md";
		await File.WriteAllTextAsync(Path.Combine(BASE_DIR, filename), contents);
		Console.WriteLine($"Wrote {filename}");
	}

	private string BuildFrontMatter(PostModel post)
	{
		// Ensure main category is first one, then sort others alphabetically
		var categories = new List<string> { post.MainCategory.Title }
			.Concat(
				post.PostCategories
					.Select(x => x.Category.Title)
					.Order()
			)
			.Distinct();

		var yaml = _yamlSerializer.Serialize(new PostFrontMatter
		{
			Id = post.Id,
			Title = post.Title,
			Published = post.Published,
			PublishedDate = post.Date,
			LastModifiedDate = post.Date,
			Summary = post.Summary,
			Categories = categories.ToList(),
		});

		// Remove blank summary if present
		return _blankSummaryRegex.Replace(yaml, string.Empty);
	}
}
