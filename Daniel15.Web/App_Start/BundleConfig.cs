using System.IO;
using System.Text.RegularExpressions;
using System.Web.Optimization;

namespace Daniel15.Web.App_Start
{
	/// <summary>
	/// Handles initialisation of JavaScript and CSS bundles
	/// </summary>
	public class BundleConfig
	{
		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
		public static void RegisterBundles(BundleCollection bundles)
		{
			RegisterCssBundles(bundles);
			RegisterJsBundles(bundles);

			//BundleTable.EnableOptimizations = true; // Forces minification even in debug builds
		}

		/// <summary>
		/// Registers the CSS bundles
		/// </summary>
		/// <param name="bundles">Bundle collection</param>
		private static void RegisterCssBundles(BundleCollection bundles)
		{
			RegisterLessBundle(bundles, new StyleBundle("~/bundles/main.css").Include("~/Content/css/main.less"));
			RegisterLessBundle(bundles, new StyleBundle("~/bundles/blogadmin.css").Include("~/Content/css/blogadmin.css"));
		}

		private static void RegisterLessBundle(BundleCollection bundles, Bundle bundle)
		{
			bundle.Transforms.Clear();
			bundle.Transforms.Add(new LessTransform());
			bundle.Transforms.Add(new CssMinify());
			bundles.Add(bundle);
		}

		/// <summary>
		/// Registers the JavaScript bundles
		/// </summary>
		/// <param name="bundles">Bundle collection</param>
		private static void RegisterJsBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/main.js").Include(
				// Framework
				"~/Content/js/framework/core.js",
				"~/Content/js/framework/ajax.js",
				"~/Content/js/framework/dom.js",
				"~/Content/js/framework/events.js",
				"~/Content/js/framework/storage.js",

				// Site scripts
				"~/Content/js/core.js",
				"~/Content/js/site.js",
				"~/Content/js/blog.js",
				"~/Content/js/socialfeed.js"));

			bundles.Add(new ScriptBundle("~/bundles/syntaxHighlight.js").Include(
				"~/Content/js/lib/syntaxhighlighter/shCore.js",
				"~/Content/js/lib/syntaxhighlighter/shBrushJScript.js",
				"~/Content/js/lib/syntaxhighlighter/shBrushPhp.js",
				"~/Content/js/lib/syntaxhighlighter/shBrushCSharp.js",
				"~/Content/js/lib/syntaxhighlighter/shBrushXml.js",
				"~/Content/js/lib/syntaxhighlighter/shBrushDelphi.js",
				"~/Content/js/lib/syntaxhighlighter/shBrushPlain.js",
				"~/Content/js/lib/syntaxhighlighter/shBrushCss.js",
				"~/Content/js/lib/syntaxhighlighter.js",

				"~/Content/js/syntaxhighlighter.js"));

			bundles.Add(new ScriptBundle("~/bundles/blogadmin.js").Include("~/Content/js/blogadmin.js"));
		}
	}

	// TODO: Move this elsewhere!!
	/// <summary>
	/// Handles transforming LESS files into minified CSS
	/// </summary>
	public class LessTransform : IBundleTransform
	{
		private readonly Regex _importRegex = new Regex("import '([^']+)'", RegexOptions.Compiled);
		/// <summary>
		/// Does stuff
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="response">The response.</param>
		public void Process(BundleContext context, BundleResponse response)
		{
			// TODO: Use a proper path resolver instead of this. Then the hacks can be removed
			var physicalBase = context.HttpContext.Request.PhysicalApplicationPath;
			var physicalPath = Path.Combine(physicalBase, "Content", "css", "");

			// First replace all the imports with a full path (C:\....\blah.less or /var/www/..../blah.less)
			response.Content = _importRegex.Replace(response.Content, match => "import '" + Path.Combine(physicalPath, match.Groups[1].Value.Replace("/", Path.DirectorySeparatorChar.ToString())) + "'");  // "import '" + physicalPath + "$1'");
			response.Content = dotless.Core.Less.Parse(response.Content);

			// Since the import paths were changed above, all images became relative to it.
			// Fix them by removing the physical path from the paths.
			response.Content = response.Content.Replace(physicalBase.Replace("\\", "/").TrimEnd('/'), string.Empty);
			response.ContentType = "text/css";
		}
	}
}