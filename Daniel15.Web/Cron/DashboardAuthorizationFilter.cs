using Hangfire.Dashboard;

namespace Daniel15.Cron
{
	public class DashboardAuthorizationFilter : IDashboardAuthorizationFilter
	{
		public bool Authorize(DashboardContext context)
		{
			var httpContext = context.GetHttpContext();
			return httpContext.User.Identity.IsAuthenticated;
		}
	}
}
