using System;
using Daniel15.BusinessLayer.Services;
using Daniel15.Infrastructure;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Runtime;

namespace Daniel15.Cron
{
	public class Program
	{
		private readonly IApplicationEnvironment _appEnv;
		private readonly IServiceCollection _serviceCollection = new ServiceCollection();
		private IServiceProvider _serviceProvider;

		public Program(IApplicationEnvironment appEnv)
		{
			_appEnv = appEnv;
		}

		public void Main(string[] args)
		{
			_serviceCollection.AddDaniel15();
			var builder = new ConfigurationBuilder(_appEnv.ApplicationBasePath)
				// This is extremely ugly, but the paths differ in dev vs in prod. 
				// Need to figure out a nicer way of doing this.
				.AddJsonFile("..\\Daniel15.Web\\config.json", optional: true)
				.AddJsonFile("../../../../../../site/approot/packages/Daniel15.Web/1.0.0/root/config.Production.json", optional: true)
				.AddEnvironmentVariables();
			_serviceCollection.AddDaniel15Config(builder.Build());
			_serviceCollection.AddOptions();
			_serviceProvider = _serviceCollection.BuildServiceProvider();

			var operation = args[0];
			switch (operation)
			{
				case "-disqus":
					_serviceProvider.GetRequiredService<IDisqusComments>().Sync();
					break;

				case "-social":
					ActivatorUtilities.CreateInstance<SocialShareUpdater>(_serviceProvider).Run();
					break;

				case "-projects":
					ActivatorUtilities.CreateInstance<ProjectUpdater>(_serviceProvider).Run();
					break;

				default:
					throw new Exception("Invalid operation '" + operation + "'");
			}
		}
	}
}
