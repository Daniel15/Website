using System.IO;
using Daniel15.Infrastructure;
using Daniel15.SimpleIdentity;
using Daniel15.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using React.AspNet;

namespace Daniel15.Web
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("config.json")
				.AddJsonFile("config.generated.json", optional: true)
				.AddJsonFile($"config.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; set; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddIdentity<SimpleIdentityUser, SimpleIdentityRole>()
				.AddSimpleIdentity<SimpleIdentityUser>(Configuration.GetSection("Auth"))
				.AddDefaultTokenProviders();

			services.AddSession();
			services.AddMvc();
			services.AddReact();
			services.AddDaniel15();
			services.AddDaniel15Config(Configuration);
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddDebug();

			if (env.IsDevelopment())
			{
				app.UseBrowserLink();
				app.UseDeveloperExceptionPage();
				loggerFactory.AddConsole(LogLevel.Information);
			}
			else
			{
				app.UseStatusCodePagesWithReExecute("/Error/Status{0}");
				app.UseExceptionHandler("/Error");
				loggerFactory.AddConsole(LogLevel.Warning);
			}

			app.UseReact(config =>
			{
				config
					.AddScript("~/Content/js/socialfeed.jsx")
					.SetReuseJavaScriptEngines(false);
			});

			app.UseStaticFiles();
			app.UseIdentity();
			app.UseSession();
			// All real routes are defined using attributes.
			app.UseMvcWithDefaultRoute();

			// This is really not ideal, need to figure out a better way to do this.
			// Based off http://stackoverflow.com/a/30762664/210370
			UrlHelperExtensions.HttpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
		}

		public static void Main(string[] args)
		{
			var config = new ConfigurationBuilder()
				.AddCommandLine(args)
				.Build();

			var host = new WebHostBuilder()
				.UseKestrel()
				.UseConfiguration(config)
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseIISIntegration()
				.UseStartup<Startup>()
				.Build();

			host.Run();
		}
	}
}
