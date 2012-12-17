using System.Web.Mvc;
using System.Web.Routing;

namespace Daniel15.Web.App_Start
{
	/// <summary>
	/// Handles initialisation of ASP.NET MVC routes
	/// </summary>
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("elmah.axd");

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

			// Viewing a category
			routes.MapRoute(
				name: "BlogCategory",
				url: "blog/category/{slug}",
				defaults: new { controller = "Blog", action = "Category", page = 1 }
			);
			routes.MapRoute(
				name: "BlogCategoryPage",
				url: "blog/category/{slug}/page-{page}",
				defaults: MVC.Blog.Category(),
				constraints: new { page = @"\d+"}
			);

			// Blog RSS feed
			routes.MapRoute(
				name: "BlogFeed",
				url: "blog/feed",
				defaults: MVC.Blog.Feed()
			);

			// Blog home page
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

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
			);

			/*
	// Viewing a tag
	Route::set('blog_tag', 'blog/tag/<slug>')
		->defaults(array(
			'controller' => 'blog',
			'action'     => 'tag',
		));
		
	// Blog sub-controllers (sidebar, feed)
	Route::set('blog_sub', 'blog/<controller>(/<action>(/<id>))', array('controller' => '(sidebar|feed)'))
		->defaults(array(
			'directory' => 'blog',
			'action'    => 'index'
		));
		
	// Blog administration
	Route::set('blogadmin', 'blogadmin(/<controller>(/<action>(/<id>)))')
		->defaults(array(
			'directory'  => 'blogadmin',
			'controller' => 'home',
			'action'     => 'index',
		));
		
	// Latest CSS and JavaScript
	Route::set('latest_js', 'res/combined/<name>.<type>')
		->defaults(array(
			'controller' => 'redirect',
			'action'     => 'latest_res',
		));	*/
		}
	}
}