using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using Daniel15.Infrastructure;
using Daniel15.Web.Areas.Screenshots.Models.Screenshot;
using Daniel15.Web.Areas.Screenshots.ViewModels.Screenshot;
using System.Linq;
using Daniel15.Shared.Extensions;

namespace Daniel15.Web.Areas.Screenshots.Controllers
{
	/// <summary>
	/// Handles browsing and serving screenshots
	/// </summary>
	public partial class ScreenshotController : Controller
	{
		/// <summary>
		/// Maximum size (width or height) for thumbnail images
		/// </summary>
		private const int MAX_THUMBNAIL_SIZE = 200;
		/// <summary>
		/// Name of the thumbnail directory
		/// </summary>
		private const string THUMBNAIL_DIR = "thumb";

		/// <summary>
		/// The site configuration
		/// </summary>
		private readonly ISiteConfiguration _siteConfiguration;

		/// <summary>
		/// Initializes a new instance of the <see cref="ScreenshotController" /> class.
		/// </summary>
		/// <param name="siteConfiguration">The site configuration.</param>
		public ScreenshotController(ISiteConfiguration siteConfiguration)
		{
			_siteConfiguration = siteConfiguration;
		}

		/// <summary>
		/// Displays a listing of screenshots or serves a screenshot file, depending on whether the
		/// request was for a file or directory.
		/// </summary>
		/// <param name="path">Path to display</param>
		/// <returns>Screenshot listing or file download</returns>
		public virtual ActionResult Index(string path = "")
		{
			var fullPath = GetAndValidateFullPath(path);

			// Don't allow access to thumbnail directory
			if (path == THUMBNAIL_DIR)
			{
				return HttpNotFound("Tried to directly access thumbnail folder");
			}

			// Directory? Return a directory listing.
			if (System.IO.Directory.Exists(fullPath))
			{
				return Directory(path, fullPath);
			}
			// File? Serve the file
			else if (System.IO.File.Exists(fullPath))
			{
				return Download(fullPath);
			}
			else
			{
				// Neither a directory or file - Bail out!
				return HttpNotFound(string.Format("Screenshot file '{0}' not found.", path));
			}
		}

		/// <summary>
		/// Displays a listing of screenshots in the specified path
		/// </summary>
		/// <param name="path">Relative path to display</param>
		/// <param name="fullPath">Full file system path</param>
		/// <returns>Directory listing</returns>
		private ActionResult Directory(string path, string fullPath)
		{
			var filesAndDirectories =
				System.IO.Directory.EnumerateDirectories(fullPath).Where(x => Path.GetFileName(x) != THUMBNAIL_DIR).Select(x => BuildScreenshotModel(x, ScreenshotFileModel.FileType.Directory))
				.Concat(System.IO.Directory.EnumerateFiles(fullPath).Select(x => BuildScreenshotModel(x, ScreenshotFileModel.FileType.File)));

			return View(Views.Index, new IndexViewModel
			{
				Path = path,
				Files = filesAndDirectories
			});
		}

		/// <summary>
		/// Returns the specified file. This is just a fallback - The directory should be configured
		/// in Nginx or IIS and served directly.
		/// </summary>
		/// <param name="fullPath">Full path of the file to serve</param>
		/// <returns>File contents</returns>
		private ActionResult Download(string fullPath)
		{
			return File(fullPath, "image/png");
		}

		/// <summary>
		/// Generates a thumbnail for the specified image
		/// </summary>
		/// <param name="path">Image to generate thumbnail for</param>
		/// <returns>Thumbnail</returns>
		public virtual ActionResult Thumbnail(string path)
		{
			var fullPath = GetAndValidateFullPath(path);
			var cachePath = Path.Combine(_siteConfiguration.ScreenshotsDir, THUMBNAIL_DIR, path);

			// Cache the thumbnail if it doesn't already exist
			if (!System.IO.File.Exists(cachePath))
			{
				System.IO.Directory.CreateDirectory(Path.GetDirectoryName(cachePath));

				using (var sourceImg = new Bitmap(fullPath))
				using (var thumb = sourceImg.GenerateThumbnail(MAX_THUMBNAIL_SIZE))
				{
					thumb.Save(cachePath, ImageFormat.Png);
				}	
			}

			return File(cachePath, "image/png");
		}

		/// <summary>
		/// Converts the specified relative path to an absolute file system path, and ensure it's 
		/// not outside the screenshots directory.
		/// </summary>
		/// <param name="path">Relative path</param>
		/// <returns>Absolute path</returns>
		private string GetAndValidateFullPath(string path)
		{
			var root = _siteConfiguration.ScreenshotsDir;
			var fullPath = Path.Combine(root, path);

			// The URI class handles normalising the path (eg. c:\Blah\..\foo\ --> c:\foo)
			// After normalisation, check that it's under the root
			var inRoot = new Uri(fullPath).LocalPath.StartsWith(root);
			if (!inRoot)
			{
				throw new Exception("Tried to access path outside root. '" + path + "'");
			}

			return fullPath;
		}

		/// <summary>
		/// Builds a screenshot model from the specified path
		/// </summary>
		/// <param name="path">Path to the file or directory</param>
		/// <param name="type">Type of the entity</param>
		/// <returns>Screenshot model</returns>
		private ScreenshotFileModel BuildScreenshotModel(string path, ScreenshotFileModel.FileType type)
		{
			var relativePath = path
				.Replace(_siteConfiguration.ScreenshotsDir, string.Empty)
				.TrimStart(Path.DirectorySeparatorChar);

			return new ScreenshotFileModel
			{
				FileName = Path.GetFileName(path),
				RelativePath = relativePath,
				Url = Url.Action(MVC.Screenshots.Screenshot.Index(relativePath.Replace('\\', '/'))),
				ThumbnailUrl = Url.Action(MVC.Screenshots.Screenshot.Thumbnail(relativePath.Replace('\\', '/'))),
				Type = type
			};
		}
	}
}
