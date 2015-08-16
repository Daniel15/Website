using System.Threading.Tasks;
using Daniel15.Infrastructure;
using Daniel15.SimpleIdentity;
using Daniel15.Web.Extensions;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Runtime;
using React.AspNet;

namespace Daniel15.Web
{
	public class Startup
	{
		public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
		{
			var builder = new ConfigurationBuilder(appEnv.ApplicationBasePath)
				.AddJsonFile("config.json")
				.AddJsonFile("config.generated.json", optional: true)
				.AddJsonFile($"config.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfiguration Configuration { get; set; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddIdentity<SimpleIdentityUser, SimpleIdentityRole>()
				.AddSimpleIdentity<SimpleIdentityUser>(Configuration.GetConfigurationSection("Auth"))
				.AddDefaultTokenProviders();

			services.AddCaching();
			services.AddSession();
			services.AddMvc();
			services.AddReact();
			services.AddDaniel15();
			services.AddDaniel15Config(Configuration);
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			if (env.IsDevelopment())
			{
				app.UseBrowserLink();
				app.UseErrorPage();
				loggerFactory.AddConsole(LogLevel.Information);
			}
			else
			{
				app.UseStatusCodePagesWithReExecute("/Error/Status{0}");
				app.UseErrorHandler("/Error");
				loggerFactory.AddConsole(LogLevel.Warning);
			}

			app.UseReact(config =>
			{
				config
					.AddScript("~/Content/js/socialfeed.jsx")
					.SetUseHarmony(true)
					.SetReuseJavaScriptEngines(false);
			});

			app.UseStaticFiles();
			app.UseIdentity();
			app.UseSession();
			// All real routes are defined using attributes.
			app.UseMvcWithDefaultRoute();


			// Don't fall back to IIS on 404.
			// TODO: This won't be needed from beta7 onwards: https://github.com/aspnet/Announcements/issues/54
			app.Run(context =>
			{
				context.Response.StatusCode = 404;
				return Task.FromResult(0);
			});

			// This is really not ideal, need to figure out a better way to do this.
			// Based off http://stackoverflow.com/a/30762664/210370
			UrlHelperExtensions.HttpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
		}
	}
}
