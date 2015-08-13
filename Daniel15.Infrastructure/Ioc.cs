using Daniel15.BusinessLayer;
using Daniel15.BusinessLayer.Services;
using Daniel15.BusinessLayer.Services.CodeRepositories;
using Daniel15.BusinessLayer.Services.Social;
using Daniel15.Data;
using Daniel15.Data.Repositories;
using Daniel15.Data.Repositories.EntityFramework;
using Daniel15.Shared.Configuration;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.OptionsModel;

namespace Daniel15.Infrastructure
{
	/// <summary>
	/// Handles initialisation of the IoC container.
	/// </summary>
	public static class Ioc
	{
		public static void AddDaniel15(this IServiceCollection services)
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

			// TODO
			// _siteConfig.ApiKeys = (ApiKeysConfiguration)ConfigurationManager.GetSection("ApiKeys");
			// Container.Register<IGalleryConfiguration>(() => (GalleryConfiguration)ConfigurationManager.GetSection("Gallery"));

			InitializeDatabase(services);
		}

		public static void AddDaniel15Config(this IServiceCollection services, IConfiguration config)
		{
			services.AddSingleton(_ => config);
			services.Configure<SiteConfiguration>(config.GetConfigurationSection("Site"));
			services.Configure<GalleryConfiguration>(config.GetConfigurationSection("Gallery"));
			services.AddSingleton<ISiteConfiguration>(
				provider => provider.GetRequiredService<IOptions<SiteConfiguration>>().Options
			);
			services.AddSingleton<IGalleryConfiguration>(
				provider => provider.GetRequiredService<IOptions<GalleryConfiguration>>().Options
			);
		}

		/// <summary>
		/// Initialises the database stuff in the IoC container
		/// </summary>
		private static void InitializeDatabase(IServiceCollection services)
		{
			services.AddScoped<DatabaseContext>();
			services.AddScoped<IBlogRepository, BlogRepository>();
			services.AddScoped<IDisqusCommentRepository, DisqusCommentRepository>();
			services.AddScoped<IProjectRepository, ProjectRepository>();
			services.AddScoped<IMicroblogRepository, MicroblogRepository>();
		}
	}
}
