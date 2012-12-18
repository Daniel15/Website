using System.Web.Mvc;
using Daniel15.Web.Attributes;
using HandleErrorAttribute = Daniel15.Web.Attributes.HandleErrorAttribute;

namespace Daniel15.Web.App_Start
{
	/// <summary>
	/// Handles initialisation of ASP.NET MVC filters
	/// </summary>
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
			filters.Add(new StopwatchAttribute());
		}
	}
}