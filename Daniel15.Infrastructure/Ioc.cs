using Daniel15.BusinessLayer;
using Daniel15.BusinessLayer.Services;
using Daniel15.BusinessLayer.Services.CodeRepositories;
using Daniel15.BusinessLayer.Services.Social;
using Daniel15.Data;
using Daniel15.Data.Repositories;
using Daniel15.Data.Repositories.EntityFramework;
using Daniel15.Shared.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Daniel15.Infrastructure
{
	/// <summary>
	/// Handles initialisation of the IoC container.
	/// </summary>
	public static class Ioc
	{
		public static void AddDaniel15(this IServiceCollection services, IConfiguration config)
		{
			// Services
			services.AddSingleton<IUrlShortener, UrlShortener>();
			services.AddSingleton<ISocialManager, SocialManager>();
			services.AddSingleton<IDisqusComments, DisqusComments>();
			services.AddSingleton<IMarkdownProcessor, MarkdownProcessor>();
			services.AddSingleton<IProjectCacheUpdater, ProjectCacheUpdater>();
			services.AddSingleton<ICodeRepositoryManager, CodeRepositoryManager>();
			services.AddSingleton<ICodeRepository, GithubCodeRepository>();
			services.AddSingleton<Facebook>();
			services.AddSingleton<Reddit>();
			services.AddSingleton<Twitter>();
			services.AddSingleton<Linkedin>();

			InitializeDatabase(services, config);
		}

		public static void AddDaniel15Config(this IServiceCollection services, IConfiguration config)
		{
			services.AddSingleton(_ => config);
			services.Configure<SiteConfiguration>(config.GetSection("Site"));
			services.Configure<GalleryConfiguration>(config.GetSection("Gallery"));
			services.AddSingleton<ISiteConfiguration>(
				provider => provider.GetRequiredService<IOptions<SiteConfiguration>>().Value
			);
			services.AddSingleton<IGalleryConfiguration>(
				provider => provider.GetRequiredService<IOptions<GalleryConfiguration>>().Value
			);
		}

		/// <summary>
		/// Initialises the database stuff in the IoC container
		/// </summary>
		private static void InitializeDatabase(IServiceCollection services, IConfiguration config)
		{
			services.AddDbContext<DatabaseContext>(options =>
				options.UseMySql(config["Data:DefaultConnection:ConnectionString"])
			);
			services.AddScoped<IBlogRepository, BlogRepository>();
			services.AddScoped<IDisqusCommentRepository, DisqusCommentRepository>();
			services.AddScoped<IProjectRepository, ProjectRepository>();
			services.AddScoped<IMicroblogRepository, MicroblogRepository>();
		}
	}
}
