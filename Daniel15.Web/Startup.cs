using Daniel15.Infrastructure;
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
			services.AddSingleton<ISiteConfiguration>(
				provider => provider.GetRequiredService<IOptions<SiteConfiguration>>().Options
			);
			services.AddSingleton<IConfiguration>(_ => Configuration);

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
			app.UseMvc(routes =>
			{
				// Normal pages on the website
				routes.MapRoute(
					name: "Page",
					template: "{action}.htm",
					defaults: new { controller = "Site" }
				);

				// Signature images
				routes.MapRoute(
					name: "Signature",
					template: "sig/{action}.png",
					defaults: new { controller = "Signature" }
				);

				// When all else fails... A default route, just in case.
				routes.MapRoute(
					name: "default",
					template: "{controller=Site}/{action=Index}/{id?}"
				);
			});
		}
	}
}
