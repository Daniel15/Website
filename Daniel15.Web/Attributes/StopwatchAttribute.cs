using System.Diagnostics;
using System.Web.Mvc;

namespace Daniel15.Web.Attributes
{
	/// <summary>
	/// Handles timing controller actions and recording the exection time in a HTTP header.
	/// </summary>
	public class StopwatchAttribute : ActionFilterAttribute
	{
		/// <summary>
		/// Called by the ASP.NET MVC framework before the action method executes.
		/// </summary>
		/// <param name="filterContext">The filter context.</param>
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var stopwatch = new Stopwatch();
			filterContext.HttpContext.Items["Stopwatch"] = stopwatch;

			stopwatch.Start();
		}

		/// <summary>
		/// Called by the ASP.NET MVC framework after the action method executes.
		/// </summary>
		/// <param name="filterContext">The filter context.</param>
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			var stopwatch = (Stopwatch)filterContext.HttpContext.Items["Stopwatch"];
			stopwatch.Stop();

			var httpContext = filterContext.HttpContext;
			var response = httpContext.Response;

			response.AddHeader("X-ExecutionTime", stopwatch.Elapsed.ToString());
		}
	}
}