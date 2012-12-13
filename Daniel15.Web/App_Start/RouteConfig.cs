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
	
			// Viewing a blog post
			routes.MapRoute(
				name: "BlogView",
				url: "blog/{year}/{month}/{slug}",
				defaults: new { controller = "Blog", action = "View" },
				constraints: new { year = @"\d{4}", month = @"\d{2}" }
			);

			// Blog monthly archive
			routes.MapRoute(
				name: "BlogArchive",
				url: "blog/{year}/{month}",
				defaults: new { controller = "Blog", action = "Archive" },
				constraints: new { year = @"\d{4}", month = @"\d{2}" }
			);

			// Viewing a category
			routes.MapRoute(
				name: "BlogCategory",
				url: "blog/category/{slug}",
				defaults: new { controller = "Blog", action = "Category" }
			);

			// Blog home page
			routes.MapRoute(
				name: "BlogHome",
				url: "blog",
				defaults: new { controller = "Blog", action = "Index" }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
			);

			/*	// Unsubscribe from blog comment emails
	Route::set('blog_unsub', 'blog/<year>/<month>/<slug>/unsub/<email>', array('year' => '\d{4}', 'month' => '\d{2}', 'email' => '.+'))
		->defaults(array(
			'controller' => 'blog',
			'action'     => 'unsub',
		));
	// Viewing a category
	Route::set('blog_category', 'blog/category/<slug>')
		->defaults(array(
			'controller' => 'blog',
			'action'     => 'category',
		));
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
		
	Route::set('blog_home', 'blog')
		->defaults(array(
			'controller' => 'blog',
			'action'     => 'index',
		));

	// Blog short URLs
	Route::set('blog_short_url', 'B<alias>')
		->defaults(array(
			'controller' => 'blog',
			'action'     => 'short_url',
		));
		
	// Blog administration
	Route::set('blogadmin', 'blogadmin(/<controller>(/<action>(/<id>)))')
		->defaults(array(
			'directory'  => 'blogadmin',
			'controller' => 'home',
			'action'     => 'index',
		));
		
	// Errors
	Route::set('error', 'error/<action>(/<message>)', array('action' => '[0-9]++', 'message' => '.+'))
		->defaults(array(
			'controller' => 'error'
		));	
		
	// Latest CSS and JavaScript
	Route::set('latest_js', 'res/combined/<name>.<type>')
		->defaults(array(
			'controller' => 'redirect',
			'action'     => 'latest_res',
		));	
		
	Route::set('default', '(<controller>(/<action>(/<id>)))')
		->defaults(array(
			'controller' => 'site',
			'action'     => 'index',
		));*/
		}
	}
}