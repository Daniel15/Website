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
			bundles.Add<StylesheetBundle>("main",
				"~/Content/css/style.less", 
				"~/Content/css/sprites-processed.less", 
				"~/Content/css/blog.less", 

				//<!-- Pages -->
				"~/Content/css/pages/index.less", 
				"~/Content/css/pages/socialfeed.less", 
				"~/Content/css/pages/projects.less", 
				"~/Content/css/pages/search.less", 

				//<!-- Mobile stylesheets -->
				"~/Content/css/mobile.less", 

				//<!-- Print stylesheets -->
				"~/Content/css/print.less", 

				//<!-- IE hacks -->
				"~/Content/css/style-ie8.less", 
				"~/Content/css/style-ie7.less", 
				"~/Content/css/style-ie6.less", 

				//<!-- Third-party -->
				//<!-- TODO: Move from JS folder -->
				"~/Content/js/lib/syntaxhighlighter/shCore.less", 
				"~/Content/js/lib/syntaxhighlighter/shThemeDefault.less");

            // To combine files, try something like this instead:
            //   bundles.Add<StylesheetBundle>("Content");
            // In production mode, all of ~/Content will be combined into a single bundle.
            
            // If you want a bundle per folder, try this:
            //   bundles.AddPerSubDirectory<ScriptBundle>("Scripts");
            // Each immediate sub-directory of ~/Scripts will be combined into its own bundle.
            // This is useful when there are lots of scripts for different areas of the website.
        }
    }
}