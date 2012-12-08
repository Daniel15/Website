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

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}