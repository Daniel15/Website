using Daniel15.Web.Services.CodeRepositories;
using Daniel15.Web.Services.Social;
using Daniel15.Web.Services;
using Daniel15.Cron;
using Daniel15.SimpleIdentity;
using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Daniel15.BusinessLayer;
using MaxMind.GeoIP2;
using Daniel15.Web.Repositories.EntityFramework;
using Daniel15.Web.Repositories;
using Daniel15.Web.Zurl;
using Microsoft.EntityFrameworkCore;
using Daniel15.Shared.Configuration;
using Daniel15.Web;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
	.AddJsonFile("config.json", optional: false, reloadOnChange: true)
	.AddJsonFile("config.generated.json", optional: true)
	.AddJsonFile($"config.{builder.Environment.EnvironmentName}.json", optional: true);

var services = builder.Services;
var config = builder.Configuration;
services.AddControllersWithViews();
services.AddIdentity<SimpleIdentityUser, SimpleIdentityRole>()
	.AddSimpleIdentity<SimpleIdentityUser>(config.GetSection("Auth"))
	.AddDefaultTokenProviders();

services.AddSession();

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
{
	var path = builder.Environment.IsEnvironment("Production")
		? "/var/lib/GeoIP"
		: builder.Environment.ContentRootPath;
	return new DatabaseReader(
		Path.Combine(path, "GeoLite2-Country.mmdb")
	);
});

// HttpClient should be reused wherever possible.
// https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
// https://stackoverflow.com/questions/11178220/is-httpclient-safe-to-use-concurrently
services.AddSingleton<HttpClient>();

// Database
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

// Config
services.AddSingleton(_ => config);
services.Configure<SiteConfiguration>(config.GetSection("Site"));
services.Configure<GalleryConfiguration>(config.GetSection("Gallery"));
services.AddSingleton<ISiteConfiguration>(
	provider => provider.GetRequiredService<IOptions<SiteConfiguration>>().Value
);
services.AddSingleton<IGalleryConfiguration>(
	provider => provider.GetRequiredService<IOptions<GalleryConfiguration>>().Value
);

services.AddMiniProfiler(options =>
{
	options.ShouldProfile = options.ResultsAuthorize = options.ResultsListAuthorize = 
		request => request.HttpContext.User.Identity.IsAuthenticated;
}).AddEntityFramework();

services.AddHangfire(hangfireConfig => hangfireConfig
	.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
	.UseSimpleAssemblyNameTypeSerializer()
	.UseRecommendedSerializerSettings()
	.UseStorage(new MySqlStorage(
		config["Data:DefaultConnection:ConnectionString"],
		new MySqlStorageOptions
		{
			TablesPrefix = "Hangfire_"
		}))
);
services.AddHangfireServer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseStatusCodePagesWithReExecute("/Error/Status{0}");
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

// Handle X-Fowarded-Proto to know Nginx is using HTTPS
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
	ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor,
});

app.UseStaticFiles();
app.UseAuthentication();
app.UseSession();
app.UseMiniProfiler();
app.UseHangfireDashboard(options: new DashboardOptions
{
	Authorization = new[] {new DashboardAuthorizationFilter()}
});
// All real routes are defined using attributes.
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

CronScheduler.ScheduleCronjobs();
app.Run();
