using Microsoft.AspNet.Http.Extensions;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;

namespace Daniel15.Web.TagHelpers
{
	/// <summary>
	/// Performs YQL queries client-side, passing the returned data to a JavaScript function.
	/// </summary>
    public class YqlTagHelper : TagHelper
    {
		/// <summary>
		/// YQL query to perform
		/// </summary>
		public string Query { get; set; }

		/// <summary>
		/// Callback to call when data has loaded
		/// </summary>
		public string Callback { get; set; }

	    /// <summary>
	    /// Synchronously executes the <see cref="T:Microsoft.AspNet.Razor.Runtime.TagHelpers.TagHelper"/> with the given <paramref name="context"/> and
	    ///             <paramref name="output"/>.
	    /// </summary>
	    /// <param name="context">Contains information associated with the current HTML tag.</param>
	    /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
	    public override void Process(TagHelperContext context, TagHelperOutput output)
	    {
			output.TagName = "script";
		    output.SelfClosing = false;
			output.Attributes.Add("async", true);
			output.Attributes.Add("defer", true);

		    var url = "http://query.yahooapis.com/v1/public/yql" + new QueryBuilder
		    {
			    {"q", Query},
			    {"format", "json"},
			    {"env", "store://datatables.org/alltableswithkeys"},
			    {"callback", Callback}
		    };
			output.Attributes.Add("src", url);
	    }
    }
}
