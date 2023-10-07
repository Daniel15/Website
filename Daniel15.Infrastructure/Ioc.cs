using System.IO;
using System.Net.Http;
using Daniel15.BusinessLayer;
using Daniel15.BusinessLayer.Services;
using Daniel15.BusinessLayer.Services.CodeRepositories;
using Daniel15.BusinessLayer.Services.Social;
using Daniel15.Data;
using Daniel15.Data.Repositories;
using Daniel15.Data.Repositories.EntityFramework;
using Daniel15.Data.Zurl;
using Daniel15.Shared.Configuration;
using MaxMind.GeoIP2;
using Microsoft.AspNetCore.Hosting;
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
			services.AddSingleton<IMarkdownProcessor, MarkdownProcessor>();
			services.AddSingleton<ICodeRepositoryManager, CodeRepositoryManager>();
			services.AddSingleton<ICodeRepository, GithubCodeRepository>();
			services.AddSingleton<Facebook>();
			services.AddSingleton<Reddit>();
			services.AddSingleton<Twitter>();
			services.AddSingleton<Linkedin>();

			services.AddScoped<IProjectCacheUpdater, ProjectCacheUpdater>();
			services.AddScoped<IShortUrlLogger, ShortUrlLogger>();
			services.AddScoped<IDisqusComments, DisqusComments>();

			// Third-party libraries
			services.AddSingleton(provider => UAParser.Parser.GetDefault());
			services.AddSingleton<IGeoIP2DatabaseReader>(provider =>
				new DatabaseReader(Path.Combine(
					provider.GetRequiredService<IHostingEnvironment>().ContentRootPath, 
					"GeoLite2-Country.mmdb"
				))
			);

			// HttpClient should be reused wherever possible.
			// https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
			// https://stackoverflow.com/questions/11178220/is-httpclient-safe-to-use-concurrently
			services.AddSingleton<HttpClient>();

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
				options.UseMySql(
					config["Data:DefaultConnection:ConnectionString"], 
					ServerVersion.AutoDetect(config["Data:DefaultConnection:ConnectionString"])
				)
			);
			services.AddScoped<IBlogRepository, BlogRepository>();
			services.AddScoped<IDisqusCommentRepository, DisqusCommentRepository>();
			services.AddScoped<IProjectRepository, ProjectRepository>();
			services.AddScoped<IMicroblogRepository, MicroblogRepository>();

			services.AddDbContext<ZurlDatabaseContext>(options => 
				options.UseMySql(
					config.GetConnectionString("Zurl"),
					ServerVersion.AutoDetect(config.GetConnectionString("Zurl"))
				)
			);
			services.AddScoped<IUrlRepository, UrlRepository>();
		}
	}
}
