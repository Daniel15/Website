using System.Web.Mvc;
using System.Web.Routing;
using Combres;

namespace Daniel15.Web.App_Start
{
	/// <summary>
	/// Handles initialisation of ASP.NET MVC routes
	/// </summary>
	public class RouteConfig
	{
		/// <summary>
		/// Registers routes for URLs that should be ignored
		/// </summary>
		/// <param name="routes">The route collection to add routes to</param>
		private static void RegisterIgnoreRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("elmah.axd");
		}

		/// <summary>
		/// Register routes relating to general site sections
		/// </summary>
		/// <param name="routes">The route collection to add routes to</param>
		private static void RegisterSiteRoutes(RouteCollection routes)
		{
			// Index page
			routes.MapRoute(
				name: "Index",
				url: "",
				defaults: MVC.Site.Index()
			);

			// Normal pages on the website
			routes.MapRoute(
				name: "Page",
				url: "{action}.htm",
				defaults: new { controller = "Site" }
			);

			// Sitemap
			routes.MapRoute(
				name: "Sitemap",
				url: "sitemap.xml",
				defaults: new { controller = "Feed", action = "Sitemap" }
			);

			// Signature images
			routes.MapRoute(
				name: "Signature",
				url: "sig/{action}.png",
				defaults: new { controller = "Signature" }
			);
		}

		/// <summary>
		/// Register routes relating to the blog
		/// </summary>
		/// <param name="routes">The route collection to add routes to</param>
		private static void RegisterBlogRoutes(RouteCollection routes)
		{
			// Blog social network sharing counts
			routes.MapRoute(
				name: "BlogPostShareCount",
				url: "blog/{year}/{month}/{slug}/sharecount",
				defaults: MVC.Social.PostShareCount(),
				constraints: new { year = @"\d{4}", month = @"\d{2}" }
			);

			// Viewing a blog post
			routes.MapRoute(
				name: "BlogView",
				url: "blog/{year}/{month}/{slug}",
				defaults: MVC.Blog.View(),
				constraints: new { year = @"\d{4}", month = @"\d{2}" }
			);

			// Blog monthly archive
			routes.MapRoute(
				name: "BlogArchive",
				url: "blog/{year}/{month}",
				defaults: MVC.Blog.Archive(),
				constraints: new { year = @"\d{4}", month = @"\d{2}" }
			);

			// RSS feed for a category
			routes.MapRoute(
				name: "BlogSubCategoryFeed",
				url: "blog/category/{parentSlug}/{slug}.rss",
				defaults: MVC.Feed.BlogCategory()
			);
			routes.MapRoute(
				name: "BlogCategoryFeed",
				url: "blog/category/{slug}.rss",
				defaults: MVC.Feed.BlogCategory()
			);

			// Viewing a category
			routes.MapRoute(
				name: "BlogCategory",
				url: "blog/category/{slug}",
				defaults: new { controller = "Blog", action = "Category", page = 1 }
			);
			// This route needs to be above the subcategory route otherwise "page-{page}" will be
			// matched as a subcategory slug.
			routes.MapRoute(
				name: "BlogCategoryPage",
				url: "blog/category/{slug}/page-{page}",
				defaults: MVC.Blog.Category(),
				constraints: new { page = @"\d+" }
			);

			// Viewing a subcategory
			routes.MapRoute(
				name: "BlogSubCategory",
				url: "blog/category/{parentSlug}/{slug}",
				defaults: new { controller = "Blog", action = "Category", page = 1 }
			);
			routes.MapRoute(
				name: "BlogSubCategoryPage",
				url: "blog/category/{parentSlug}/{slug}/page-{page}",
				defaults: MVC.Blog.Category(),
				constraints: new { page = @"\d+" }
			);

			// Viewing a tag
			routes.MapRoute(
				name: "BlogTag",
				url: "blog/tag/{slug}",
				defaults: new { controller = "Blog", action = "Tag", page = 1 }
			);
			routes.MapRoute(
				name: "BlogTagPage",
				url: "blog/tag/{slug}/page-{page}",
				defaults: MVC.Blog.Tag(),
				constraints: new { page = @"\d+" }
			);

			// Blog RSS feed
			routes.MapRoute(
				name: "BlogFeed",
				url: "blog/feed",
				defaults: MVC.Feed.BlogLatest()
			);

			// Blog home page - Don't include page number in URL for first page
			routes.MapRoute(
				name: "BlogHome",
				url: "blog",
				defaults: new { controller = "Blog", action = "Index", page = 1 }
			);
			routes.MapRoute(
				name: "BlogHomePage",
				url: "blog/page-{page}",
				defaults: MVC.Blog.Index(),
				constraints: new { page = @"\d+" }
			);

			// Blog short URLs
			routes.MapRoute(
				name: "BlogShortUrl",
				url: "B{alias}",
				defaults: MVC.Blog.ShortUrl(),
				constraints: new { alias = @"[0-9A-Za-z\-_]+" }
			);
		}

		/// <summary>
		/// Register any miscellaneous routes
		/// </summary>
		/// <param name="routes">The route collection to add routes to</param>
		private static void RegisterMiscRoutes(RouteCollection routes)
		{
			// Redirects to latest CSS and JS
			routes.MapRoute(
				name: "LatestCSS",
				url: "latest.css",
				defaults: new { controller = "Redirect", action = "Css" }
			);
			routes.MapRoute(
				name: "LatestJS",
				url: "latest.js",
				defaults: new { controller = "Redirect", action = "Js" }
			);
		}

		/// <summary>
		/// Register all MVC routes for the site
		/// </summary>
		/// <param name="routes">The route collection to add routes to</param>
		public static void RegisterRoutes(RouteCollection routes)
		{
			// Don't use controllers in custom areas with these routes - Use default namespace
			ControllerBuilder.Current.DefaultNamespaces.Add("Daniel15.Web.Controllers");

			RouteTable.Routes.AddCombresRoute("Combres");

			RegisterIgnoreRoutes(routes);
			RegisterSiteRoutes(routes);
			RegisterBlogRoutes(routes);
			RegisterMiscRoutes(routes);

			// When all else fails... A default route, just in case.
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}