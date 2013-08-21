using System.Web.Mvc;

namespace Daniel15.Web.Areas.Gallery
{
	public class GalleryAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get { return "Gallery"; }
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			/* All galleries listed here will have a "shortcut" route. Instead of using 
			 * /gallery/{galleryName}/..., they just use /{galleryName}/... These are all explicitly
			 * whitelisted to prevent conflicts with other parts of the site.
			 */
			var galleryShortcuts = new[] { "screenshots" };
			var shortcutConstraint = "(" + string.Join("|", galleryShortcuts) + ")";

			context.MapRoute(
				name: "GalleryShortcutThumbnail",
				url: "{galleryName}/thumb/{*path}",
				defaults: MVC.Gallery.Gallery.Thumbnail(),
				constraints: new { galleryName = shortcutConstraint }
			);

			context.MapRoute(
				name: "GalleryShortcutHome",
				url: "{galleryName}/{*path}",
				defaults: MVC.Gallery.Gallery.Index(),
				constraints: new { galleryName = shortcutConstraint }
			);

			context.MapRoute(
				name: "GalleryThumbnail",
				url: "gallery/{galleryName}/thumb/{*path}",
				defaults: MVC.Gallery.Gallery.Thumbnail()
			);

			context.MapRoute(
				name: "GalleryHome",
				url: "gallery/{galleryName}/{*path}",
				defaults: MVC.Gallery.Gallery.Index()
			);

			context.MapRoute(
				name: "GalleryDefault",
				url: "Gallery/{controller}/{action}/{id}",
				defaults: new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
