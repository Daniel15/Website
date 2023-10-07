using System;
using System.IO;
using System.Linq;
using Daniel15.Cron;
using Daniel15.Infrastructure;
using Daniel15.SimpleIdentity;
using Hangfire;
using Hangfire.MySql;
using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Core;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using React.AspNet;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
	.AddJsonFile("config.json", optional: false, reloadOnChange: true)
	.AddJsonFile("config.generated.json", optional: true)
	.AddJsonFile($"config.{builder.Environment.EnvironmentName}.json", optional: true);

builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<SimpleIdentityUser, SimpleIdentityRole>()
	.AddSimpleIdentity<SimpleIdentityUser>(builder.Configuration.GetSection("Auth"))
	.AddDefaultTokenProviders();

builder.Services.AddSession();
JsEngineSwitcher.Current.EngineFactories.Add(new ChakraCoreJsEngineFactory());
builder.Services.AddReact();
builder.Services.AddDaniel15(builder.Configuration);
builder.Services.AddDaniel15Config(builder.Configuration);

builder.Services.AddMiniProfiler(options =>
{
	options.ShouldProfile = options.ResultsAuthorize = options.ResultsListAuthorize = 
		request => request.HttpContext.User.Identity.IsAuthenticated;
}).AddEntityFramework();

builder.Services.AddHangfire(config => config
	.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
	.UseSimpleAssemblyNameTypeSerializer()
	.UseRecommendedSerializerSettings()
	.UseStorage(new MySqlStorage(
		builder.Configuration["Data:DefaultConnection:ConnectionString"],
		new MySqlStorageOptions
		{
			TablesPrefix = "Hangfire_"
		}))
);
builder.Services.AddHangfireServer();

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

app.UseReact(config =>
{
	config
		.AddScript("~/Content/js/socialfeed.jsx")
		.SetReuseJavaScriptEngines(false);
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
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}"
);

CronScheduler.ScheduleCronjobs();
app.Run();
