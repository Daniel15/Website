using System.Web.Mvc;
using Elmah;

namespace Daniel15.Web.Attributes
{
	/// <summary>
	/// Handles uncaught errors in the site, and logs them with ELMAH
	/// </summary>
	public class HandleErrorAttribute : System.Web.Mvc.HandleErrorAttribute
	{
		/// <summary>
		/// Called when an exception occurs
		/// </summary>
		/// <param name="context">The context.</param>
		public override void OnException(ExceptionContext context)
		{
			base.OnException(context);
			if (!context.ExceptionHandled)
				return;
			var httpContext = context.HttpContext.ApplicationInstance.Context;
			var signal = ErrorSignal.FromContext(httpContext);
			signal.Raise(context.Exception, httpContext);
		}
	}
}