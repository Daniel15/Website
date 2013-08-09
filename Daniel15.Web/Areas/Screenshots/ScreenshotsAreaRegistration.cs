using System.Web.Mvc;

namespace Daniel15.Web.Areas.Screenshots
{
	public class ScreenshotsAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Screenshots";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				name: "ScreenshotThumbnail",
				url: "screenshots/thumb/{*path}",
				defaults: MVC.Screenshots.Screenshot.Thumbnail()
			);

			context.MapRoute(
				name: "ScreenshotHome",
				url: "screenshots/{*path}",
				defaults: MVC.Screenshots.Screenshot.Index()
			);

			context.MapRoute(
				"ScreenshotDefault",
				"Screenshots/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
