using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Web.Mvc;

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
			// Normal pages on the website
			routes.MapRoute(
				name: "Page",
				url: "{action}.htm",
				defaults: new { controller = "Site" }
			);

			// Signature images
			routes.MapRoute(
				name: "Signature",
				url: "sig/{action}.png",
				defaults: new { controller = "Signature" }
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

			routes.MapAttributeRoutes();

			RegisterIgnoreRoutes(routes);
			RegisterSiteRoutes(routes);

			// When all else fails... A default route, just in case.
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}