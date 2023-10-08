using Daniel15.Cron;
using Daniel15.Infrastructure;
using Daniel15.SimpleIdentity;
using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;

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
