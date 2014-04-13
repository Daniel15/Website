using Cassette;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace Daniel15.Web
{
    /// <summary>
    /// Configures the Cassette asset bundles for the web application.
    /// </summary>
    public class CassetteBundleConfiguration : IConfiguration<BundleCollection>
    {
        public void Configure(BundleCollection bundles)
        {
			bundles.Add<StylesheetBundle>("main.css",
				"~/Content/css/style.less",
				"~/Content/css/sprites-processed.less",
				"~/Content/css/blog.less",

				// Pages
				"~/Content/css/pages/index.less",
				"~/Content/css/pages/socialfeed.less",
				"~/Content/css/pages/projects.less",
				"~/Content/css/pages/search.less",

				// Mobile stylesheets
				"~/Content/css/mobile.less",

				//Print stylesheets
				"~/Content/css/print.less",

				// IE hacks
				"~/Content/css/style-ie8.less",
				"~/Content/css/style-ie7.less",
				"~/Content/css/style-ie6.less",

				// Third-party
				// TODO: Move from JS folder
				"~/Content/js/lib/syntaxhighlighter/shCore.less",
				"~/Content/js/lib/syntaxhighlighter/shThemeDefault.less"
			);

			bundles.Add<StylesheetBundle>("blogadmin.css", "~/Content/css/blogadmin.less");

			bundles.Add<ScriptBundle>("main.js",
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
				"~/Content/js/socialfeed.js"
			);

			bundles.Add<ScriptBundle>("syntaxHighlighter.js",
				"~/Content/js/lib/syntaxhighlighter/shCore.js",
				"~/Content/js/lib/syntaxhighlighter/shBrushJScript.js",
				"~/Content/js/lib/syntaxhighlighter/shBrushPhp.js",
				"~/Content/js/lib/syntaxhighlighter/shBrushCSharp.js",
				"~/Content/js/lib/syntaxhighlighter/shBrushXml.js",
				"~/Content/js/lib/syntaxhighlighter/shBrushDelphi.js",
				"~/Content/js/lib/syntaxhighlighter/shBrushPlain.js",
				"~/Content/js/lib/syntaxhighlighter/shBrushCss.js",
				"~/Content/js/syntaxhighlighter.js"
			);

			bundles.Add<ScriptBundle>("blogadmin.js", "~/Content/js/blogadmin.js");
        }
    }
}