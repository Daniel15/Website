using Daniel15.Configuration;
using Daniel15.Infrastructure;
using Daniel15.SimpleIdentity;
using Daniel15.Web.Extensions;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Runtime;

namespace Daniel15.Web
{
	public class Startup
	{
		public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
		{
			var builder = new ConfigurationBuilder(appEnv.ApplicationBasePath)
				.AddJsonFile("config.json")
				.AddJsonFile($"config.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfiguration Configuration { get; set; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<SiteConfiguration>(Configuration.GetConfigurationSection("Site"));
			services.Configure<GalleryConfiguration>(Configuration.GetConfigurationSection("Gallery"));
			services.AddSingleton<ISiteConfiguration>(
				provider => provider.GetRequiredService<IOptions<SiteConfiguration>>().Options
			);
			services.AddSingleton<IGalleryConfiguration>(
				provider => provider.GetRequiredService<IOptions<GalleryConfiguration>>().Options
			);
			services.AddSingleton<IConfiguration>(_ => Configuration);

			services.AddIdentity<SimpleIdentityUser, SimpleIdentityRole>()
				.AddSimpleIdentity<SimpleIdentityUser>(Configuration.GetConfigurationSection("Auth"))
				.AddDefaultTokenProviders();

			services.AddCaching();
			services.AddSession();
			services.AddMvc();
			services.AddDaniel15();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.MinimumLevel = LogLevel.Information;
			loggerFactory.AddConsole();

			// Configure the HTTP request pipeline.

			// Add the following to the request pipeline only in development environment.
			if (env.IsDevelopment())
			{
				app.UseBrowserLink();
				app.UseErrorPage();
			}
			else
			{
				// Add Error handling middleware which catches all application specific errors and
				// send the request to the following path or controller action.
				app.UseErrorHandler("/Site/Error");
			}

			app.UseStaticFiles();
			app.UseIdentity();
			app.UseSession();
			app.UseMvc(routes =>
			{
				// The only routes here are default routes, and routes that can not be expressed
				// with attribute routing. Most routes are defined using attributes.

				// Normal pages on the website
				routes.MapRoute(
					name: "GalleryDefault",
					template: "Gallery/{controller}/{action=Index}",
					defaults: new { area = "Gallery" }
				);

				routes.MapRoute(
					name: "BlogAdminPostsPublished",
					template: "blog/admin/posts/published",
					defaults: new { area = "Admin", controller = "Blog", action = "Posts", published = "true" }
				);

				routes.MapRoute(
					name: "BlogAdminPostsUnpublished",
					template: "blog/admin/posts/unpublished",
					defaults: new { area = "Admin", controller = "Blog", action = "Posts", published = "false" }
				);

				routes.MapRoute(
					name: "BlogAdminDefault",
					template: "blog/admin/{action=Index}",
					defaults: new { area = "Admin", controller = "Blog" }
				);

				routes.MapRoute(
					name: "AdminDefault",
					template: "Admin/{controller}/{action=Index}",
					defaults: new { area = "Admin" }
				);

				// When all else fails... A default route, just in case.
				routes.MapRoute(
					name: "default",
					template: "{controller=Site}/{action=Index}/{id?}"
				);
			});


			// This is really not ideal, need to figure out a better way to do this.
			// Based off http://stackoverflow.com/a/30762664/210370
			UrlHelperExtensions.HttpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
		}
	}
}
