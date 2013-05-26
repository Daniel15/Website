using System;
using Daniel15.BusinessLayer.Services;
using Daniel15.Infrastructure;

namespace Daniel15.Cron
{
	class CronRunner
	{
		static void Main(string[] args)
		{
			new CronRunner().Run(args[1]);
		}

		public CronRunner()
		{
			Ioc.Initialise();
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

				default:
					throw new Exception("Invalid operation '" + operation + "'");
			}
		}
	}
}
