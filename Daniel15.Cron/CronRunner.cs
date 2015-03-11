using System;
using Daniel15.BusinessLayer.Services;
using Daniel15.Infrastructure;
using SimpleInjector;

namespace Daniel15.Cron
{
	class CronRunner
	{
		static void Main(string[] args)
		{
			new CronRunner().Run(args[0]);
		}

		public CronRunner()
		{
			Ioc.Initialise(dbLifestyle: Lifestyle.Singleton);
		}

		private void Run(string operation)
		{
			switch (operation)
			{
				case "-disqus":
					Ioc.Container.GetInstance<IDisqusComments>().Sync();
					break;

				case "-social":
					Ioc.Container.GetInstance<SocialShareUpdater>().Run();
					break;

				case "-projects":
					Ioc.Container.GetInstance<ProjectUpdater>().Run();
					break;

				default:
					throw new Exception("Invalid operation '" + operation + "'");
			}
		}
	}
}
