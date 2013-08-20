using System.Web.Mvc;

namespace Daniel15.Web.Areas.Gallery
{
	public class GalleryAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Gallery";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				name: "ScreenshotThumbnail",
				url: "screenshots/thumb/{*path}",
				defaults: MVC.Gallery.Gallery.Thumbnail()
			);

			context.MapRoute(
				name: "ScreenshotHome",
				url: "screenshots/{*path}",
				defaults: MVC.Gallery.Gallery.Index()
			);

			context.MapRoute(
				"ScreenshotDefault",
				"Screenshots/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
