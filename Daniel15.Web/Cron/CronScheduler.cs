using Daniel15.Web.Services;
using Hangfire;

namespace Daniel15.Cron
{
	public static class CronScheduler
	{
		public static void ScheduleCronjobs()
		{
			RecurringJob.AddOrUpdate<IDisqusComments>(
				"disqus-sync",
				x => x.SyncAsync(),
				Hangfire.Cron.Hourly(20)
			);

			RecurringJob.AddOrUpdate<SocialShareUpdater>(
				"social-shares",
				x => x.RunAsync(),
				Hangfire.Cron.Hourly(20)
			);

			RecurringJob.AddOrUpdate<ProjectUpdater>(
				"projects",
				x => x.RunAsync(),
				Hangfire.Cron.Hourly(20)
			);
		}
	}
}
