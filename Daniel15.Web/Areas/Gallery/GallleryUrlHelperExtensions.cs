using Microsoft.AspNetCore.Mvc;

namespace Daniel15.Web.Areas.Gallery
{
	public static class GallleryUrlHelperExtensions
	{
		/// <summary>
		/// Gets the URL to the specified gallery directory.
		/// </summary>
		/// <returns></returns>
		public static string GalleryDirectory(this IUrlHelper urlHelper, string galleryName, string path)
		{
			var uri = urlHelper.Action("Index", "Gallery", new
			{
				area = "Gallery",
				galleryName,
				path,
			});

			// Workaround for https://github.com/aspnet/Routing/issues/363
			return uri.Replace("%2F", "/");
		}
	}
}
