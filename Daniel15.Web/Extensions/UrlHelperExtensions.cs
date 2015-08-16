using System;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;

namespace Daniel15.Web.Extensions
{
	/// <summary>
	/// Various URL helpers
	/// </summary>
	public static class UrlHelperExtensions
	{
		public static IHttpContextAccessor HttpContextAccessor { get; set; }

		/// <summary>
		/// Gets the URL to the specified JavaScript file
		/// </summary>
		/// <param name="urlHelper">The URL helper.</param>
		/// <param name="filename">The filename.</param>
		/// <returns>JavaScript URL</returns>
		public static string Js(this IUrlHelper urlHelper, string filename)
		{
			return urlHelper.Content("~/Content/js/" + filename);
		}

		/// <summary>
		/// Gets the URL to the specified image
		/// </summary>
		/// <param name="urlHelper">The URL helper.</param>
		/// <param name="filename">The filename.</param>
		/// <returns>Image URL</returns>
		public static string Image(this IUrlHelper urlHelper, string filename)
		{
			return urlHelper.Content("~/Content/images/" + filename);
		}

		/// <summary>
		/// Converts the specified relative URI into an absolute URL
		/// </summary>
		/// <param name="urlHelper">URL helper</param>
		/// <param name="uri">Relative URI</param>
		/// <returns>Absoute URL</returns>
		public static string Absolute(this IUrlHelper urlHelper, string uri)
		{
			var request = HttpContextAccessor.HttpContext.Request;
			var hostAndPort = request.Host.Value.Split(':');
            var builder = new UriBuilder
			{
				Host = hostAndPort[0],
				Scheme = request.Scheme,
				Path = uri
			};
			if (hostAndPort.Length == 2)
			{
				builder.Port = int.Parse(hostAndPort[1]);
			}
			return builder.ToString();
		}

		/// <summary>
		/// Gets the absolute URL (http://..../blah) to the specified URI
		/// </summary>
		/// <param name="urlHelper">The URL helper</param>
		/// <param name="uri">URI to link to</param>
		/// <returns>The absolute URL</returns>
		public static string ContentAbsolute(this IUrlHelper urlHelper, string uri)
		{
			return urlHelper.Absolute(urlHelper.Content(uri));
		}
	}
}